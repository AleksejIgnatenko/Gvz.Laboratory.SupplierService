using Confluent.Kafka;
using Gvz.Laboratory.SupplierService.Abstractions;
using Gvz.Laboratory.SupplierService.Dto;
using Serilog;
using System.Text.Json;

namespace Gvz.Laboratory.SupplierService.Kafka
{
    public class AddProductKafkaConsumer : IHostedService
    {
        private readonly ConsumerConfig _config;
        private IConsumer<Ignore, string> _consumer;
        private CancellationTokenSource _cts;
        private readonly IProductRepository _productRepository;

        public AddProductKafkaConsumer(ConsumerConfig config, IProductRepository productRepository)
        {
            _config = config;
            _productRepository = productRepository;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _consumer = new ConsumerBuilder<Ignore, string>(_config).Build();

            _consumer.Subscribe("add-product-topic");

            Task.Run(() => ConsumeMessages(cancellationToken));

            return Task.CompletedTask;
        }

        private async void ConsumeMessages(CancellationToken cancellationToken)
        {
            try
            {
                while (true)
                {
                    try
                    {
                        var cr = _consumer.Consume(cancellationToken);

                        var addProductDto = JsonSerializer.Deserialize<ProductDto>(cr.Message.Value)
                            ?? throw new InvalidOperationException("Deserialization failed: ManufacturerDto is null.");

                        var addProductId = await _productRepository.CreateProductAsync(addProductDto);

                    }
                    catch (ConsumeException e)
                    {
                        Log.Error($"Error occurred: {e.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _consumer.Close();
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
