using Gvz.Laboratory.SupplierService.Models;

namespace Gvz.Laboratory.SupplierService.Abstractions
{
    public interface ISupplierRepository
    {
        Task<Guid> CreateSupplierAsync(SupplierModel supplier);
        Task DeleteSupplierAsync(List<Guid> ids);
        Task<(List<SupplierModel> suppliers, int numberSuppliers)> GetSuppliersForPageAsync(int page);
        Task<Guid> UpdateSupplierAsync(SupplierModel supplier);
    }
}