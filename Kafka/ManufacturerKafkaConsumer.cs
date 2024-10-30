using Confluent.Kafka;
using Gvz.Laboratory.SupplierService.Dto;
using Serilog;
using System.Text.Json;

namespace Gvz.Laboratory.SupplierService.Kafka
{
    public class ManufacturerKafkaConsumer : IHostedService
    {
        private readonly ConsumerConfig _config;
        private IConsumer<Ignore, string> _consumer;
        private CancellationTokenSource _cts;

        public ManufacturerKafkaConsumer(ConsumerConfig config)
        {
            _config = config;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _consumer = new ConsumerBuilder<Ignore, string>(_config).Build();
            _consumer.Subscribe("add-manufacturer-topic");

            Task.Run(() => ConsumeMessages());

            return Task.CompletedTask;
        }

        private void ConsumeMessages()
        {
            try
            {
                while (true)
                {
                    try
                    {
                        var cr = _consumer.Consume();
                        var manufacturerDto = JsonSerializer.Deserialize<ManufacturerDto>(cr.Message.Value);
                        Console.WriteLine(manufacturerDto.ManufacturerName);
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
