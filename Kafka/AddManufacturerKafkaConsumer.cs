using Confluent.Kafka;
using Gvz.Laboratory.SupplierService.Abstractions;
using Gvz.Laboratory.SupplierService.Dto;
using Serilog;
using System.Text.Json;

namespace Gvz.Laboratory.SupplierService.Kafka
{
    public class AddManufacturerKafkaConsumer : IHostedService
    {
        private readonly ConsumerConfig _config;
        private IConsumer<Ignore, string> _consumer;
        private CancellationTokenSource _cts;
        private readonly IManufacturerRepository _manufacturerRepository;

        public AddManufacturerKafkaConsumer(ConsumerConfig config, IManufacturerRepository manufacturerRepository)
        {
            _config = config;
            _manufacturerRepository = manufacturerRepository;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _consumer = new ConsumerBuilder<Ignore, string>(_config).Build();

            _consumer.Subscribe("add-manufacturer-topic");

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

                        var addManufacturerDto = JsonSerializer.Deserialize<ManufacturerDto>(cr.Message.Value)
                            ?? throw new InvalidOperationException("Deserialization failed: ManufacturerDto is null.");

                        var addManufacturerId = await _manufacturerRepository.CreateManufacturerAsync(addManufacturerDto);

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
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
