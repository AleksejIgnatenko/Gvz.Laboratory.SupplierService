using Confluent.Kafka;
using Gvz.Laboratory.SupplierService;
using Gvz.Laboratory.SupplierService.Abstractions;
using Gvz.Laboratory.SupplierService.Infrastructure;
using Gvz.Laboratory.SupplierService.Kafka;
using Gvz.Laboratory.SupplierService.Middleware;
using Gvz.Laboratory.SupplierService.Repositories;
using Gvz.Laboratory.SupplierService.Services;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Serilog;

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
    GroupId = "manufacturer-group-id",
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


builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();

builder.Services.AddScoped<IManufacturerService, ManufacturerService>();
builder.Services.AddScoped<IManufacturerRepository, ManufacturerRepository>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();

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
