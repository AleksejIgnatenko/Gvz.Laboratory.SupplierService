using Gvz.Laboratory.SupplierService.Models;

namespace Gvz.Laboratory.SupplierService.Abstractions
{
    public interface ISupplierService
    {
        Task<Guid> CreateSupplierAsync(Guid id, string name, List<Guid> manufacturersIds);
        Task<List<SupplierModel>> GetSuppliersAsync();
        Task<(List<SupplierModel> suppliers, int numberSuppliers)> GetSuppliersForPageAsync(int page);
        Task<(List<SupplierModel> suppliers, int numberSuppliers)> SearchSuppliersAsync(string searchQuery, int pageNumber);
        Task<MemoryStream> ExportSuppliersToExcelAsync();
        Task DeleteSupplierAsync(List<Guid> ids);
        Task<Guid> UpdateSupplierAsync(Guid id, string name, List<Guid> manufacturersIds);
    }
}