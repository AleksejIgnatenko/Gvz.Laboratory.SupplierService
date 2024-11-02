using Gvz.Laboratory.SupplierService.Dto;
using Gvz.Laboratory.SupplierService.Entities;
using Gvz.Laboratory.SupplierService.Models;

namespace Gvz.Laboratory.SupplierService.Abstractions
{
    public interface IManufacturerRepository
    {
        Task<Guid> CreateManufacturerAsync(ManufacturerDto manufacturer);
        Task DeleteManufacturersAsync(List<Guid> ids);
        Task<List<ManufacturerModel>> GetSupplierManufacturersAsync(Guid supplierId);
        Task<(List<ManufacturerModel> manufacturers, int numberManufacturers)> GetSupplierManufacturersForPageAsync(Guid supplierId, int pageNumber);
        Task<List<ManufacturerEntity>> GetManufacturersByIdsAsync(List<Guid> manufacturersIds);
        Task<Guid> UpdateManufacturerAsync(ManufacturerDto manufacturer);
    }
}