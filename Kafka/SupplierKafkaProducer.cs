using Confluent.Kafka;
using Gvz.Laboratory.SupplierService.Abstractions;
using Gvz.Laboratory.SupplierService.Dto;
using System.Text.Json;

namespace Gvz.Laboratory.SupplierService.Kafka
{
    public class SupplierKafkaProducer : ISupplierKafkaProducer
    {
        private readonly IProducer<Null, string> _producer;

        public SupplierKafkaProducer(IProducer<Null, string> producer)
        {
            _producer = producer;
        }

        public async Task SendUserToKafka(SupplierDto supplier, string topic)
        {
            var serializedSupplier = JsonSerializer.Serialize(supplier);
            await _producer.ProduceAsync(topic, new Message<Null, string> { Value = serializedSupplier });
        }

        public async Task SendUserToKafka(List<Guid> ids, string topic)
        {
            var serializedId = JsonSerializer.Serialize(ids);
            await _producer.ProduceAsync(topic, new Message<Null, string> { Value = serializedId });
        }
    }
}
