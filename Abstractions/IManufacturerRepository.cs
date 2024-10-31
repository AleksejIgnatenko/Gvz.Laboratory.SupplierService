using Gvz.Laboratory.SupplierService.Dto;
using Gvz.Laboratory.SupplierService.Entities;

namespace Gvz.Laboratory.SupplierService.Abstractions
{
    public interface IManufacturerRepository
    {
        Task<Guid> CreateManufacturerAsync(ManufacturerDto manufacturer);
        Task DeleteManufacturersAsync(List<Guid> ids);
        Task<List<ManufacturerEntity>> GetManufacturersByIds(List<Guid> manufacturersIds);
        Task<Guid> UpdateManufacturerAsync(ManufacturerDto manufacturer);
    }
}