using Gvz.Laboratory.SupplierService.Entities;
using Gvz.Laboratory.SupplierService.Models;

namespace Gvz.Laboratory.SupplierService.Abstractions
{
    public interface ISupplierRepository
    {
        Task<Guid> CreateSupplierAsync(SupplierModel supplier, List<Guid> manufacturersIds);
        Task DeleteSupplierAsync(List<Guid> ids);
        Task<List<SupplierModel>> GetSuppliersAsync();
        Task<SupplierEntity?> GetSupplierEntityByIdAsync(Guid supplierId);
        Task<List<SupplierEntity>> GetSuppliersByIdsAsync(List<Guid> suppliersIds);
        Task<(List<SupplierModel> suppliers, int numberSuppliers)> GetSuppliersForPageAsync(int pageNumber);
        Task<(List<SupplierModel> suppliers, int numberSuppliers)> SearchSuppliersAsync(string searchQuery, int pageNumber);
        Task<Guid> UpdateSupplierAsync(SupplierModel supplier, List<Guid> manufacturersIds);
    }
}