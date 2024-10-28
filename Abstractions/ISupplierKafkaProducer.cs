using Gvz.Laboratory.SupplierService.Dto;

namespace Gvz.Laboratory.SupplierService.Abstractions
{
    public interface ISupplierKafkaProducer
    {
        Task SendUserToKafka(SupplierDto supplier, string topic);
        Task SendUserToKafka(List<Guid> ids, string topic);
    }
}