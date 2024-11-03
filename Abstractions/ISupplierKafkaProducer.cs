using Gvz.Laboratory.SupplierService.Dto;

namespace Gvz.Laboratory.SupplierService.Abstractions
{
    public interface ISupplierKafkaProducer
    {
        Task SendToKafkaAsync(SupplierDto supplier, string topic);
        Task SendToKafkaAsync(List<Guid> ids, string topic);
    }
}