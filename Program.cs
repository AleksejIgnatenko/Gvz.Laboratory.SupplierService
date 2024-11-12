using Confluent.Kafka;
using Gvz.Laboratory.SupplierService;
using Gvz.Laboratory.SupplierService.Abstractions;
using Gvz.Laboratory.SupplierService.Helpers;
using Gvz.Laboratory.SupplierService.Infrastructure;
using Gvz.Laboratory.SupplierService.Kafka;
using Gvz.Laboratory.SupplierService.Middleware;
using Gvz.Laboratory.SupplierService.Repositories;
using Gvz.Laboratory.SupplierService.Services;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add JWT bearer authentication
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
var jwtOptions = builder.Configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions?.SecretKey ?? "secretkeysecretkeysecretkeysecretkeysecretkeysecretkeysecretkey"))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["test-cookies"];
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddDbContext<GvzLaboratorySupplierServiceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSingleton(() =>
{
    var config = new TypeAdapterConfig();
    new RegisterMapper().Register(config);
    return config;
});
builder.Services.AddScoped<ISupplierMapper, SupplierMapper>();

var producerConfig = new ProducerConfig
{
    BootstrapServers = "kafka:29092"
};
builder.Services.AddSingleton<IProducer<Null, string>>(new ProducerBuilder<Null, string>(producerConfig).Build());
builder.Services.AddScoped<ISupplierKafkaProducer, SupplierKafkaProducer>();

var consumerConfig = new ConsumerConfig
{
    BootstrapServers = "kafka:29092",
    GroupId = "supplier-group-id",
    AutoOffsetReset = AutoOffsetReset.Earliest
};
builder.Services.AddSingleton(consumerConfig);

builder.Services.AddSingleton<AddManufacturerKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<AddManufacturerKafkaConsumer>());

builder.Services.AddSingleton<UpdateManufacturerKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<UpdateManufacturerKafkaConsumer>());

builder.Services.AddSingleton<DeleteManufacturerKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<DeleteManufacturerKafkaConsumer>());

builder.Services.AddSingleton<AddProductKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<AddProductKafkaConsumer>());

builder.Services.AddSingleton<UpdateProductKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<UpdateProductKafkaConsumer>());

builder.Services.AddSingleton<DeleteProductKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<DeleteProductKafkaConsumer>());

builder.Services.AddSingleton<AddPartyKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<AddPartyKafkaConsumer>());

builder.Services.AddSingleton<UpdatePartyKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<UpdatePartyKafkaConsumer>());

builder.Services.AddSingleton<DeletePartyKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<DeletePartyKafkaConsumer>());


builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();

builder.Services.AddScoped<IManufacturerService, ManufacturerService>();
builder.Services.AddScoped<IManufacturerRepository, ManufacturerRepository>();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<IPartyService, PartyService>();
builder.Services.AddScoped<IPartyRepository, PartyRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors(x =>
{
    x.WithHeaders().AllowAnyHeader();
    x.WithOrigins("http://localhost:3000");
    x.WithMethods().AllowAnyMethod();
});

app.Run();
