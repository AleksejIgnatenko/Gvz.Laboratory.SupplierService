using Gvz.Laboratory.SupplierService.Models;

namespace Gvz.Laboratory.SupplierService.Abstractions
{
    public interface ISupplierRepository
    {
        Task<Guid> CreateSupplierAsync(SupplierModel supplier);
        Task<Guid> DeleteSupplierAsync(Guid id);
        Task<(List<SupplierModel> suppliers, int numberSuppliers)> GetSuppliersForPageAsync(int page);
        Task<Guid> UpdateSupplierAsync(SupplierModel supplier);
    }
}