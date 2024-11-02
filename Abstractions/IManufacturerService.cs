using Gvz.Laboratory.SupplierService.Models;

namespace Gvz.Laboratory.SupplierService.Abstractions
{
    public interface IManufacturerService
    {
        Task<(List<ManufacturerModel> manufacturers, int numberManufacturers)> GetSupplierManufacturersForPageAsync(Guid supplierId, int pageNumber);
        Task<List<ManufacturerModel>> GetSupplierManufacturers(Guid supplierId);
    }
}