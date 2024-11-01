using Gvz.Laboratory.SupplierService.Models;

namespace Gvz.Laboratory.SupplierService.Abstractions
{
    public interface IManufacturerService
    {
        Task<List<ManufacturerModel>> GetSupplierManufacturers(Guid supplierId);
    }
}