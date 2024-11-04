using Gvz.Laboratory.SupplierService.Models;

namespace Gvz.Laboratory.SupplierService.Abstractions
{
    public interface IProductService
    {
        Task<(List<ProductModel> suppliers, int numberSuppliers)> GetSupplierProductsForPageAsync(Guid supplierId, int pageNumber);
    }
}