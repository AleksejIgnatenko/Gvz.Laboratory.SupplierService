using Gvz.Laboratory.SupplierService.Dto;
using Gvz.Laboratory.SupplierService.Models;

namespace Gvz.Laboratory.SupplierService.Abstractions
{
    public interface IProductRepository
    {
        Task<Guid> CreateProductAsync(ProductDto product);
        Task DeleteProductsAsync(List<Guid> ids);
        Task<List<ProductModel>> GetSupplierProductsAsync(Guid supplierId);
        Task<(List<ProductModel> products, int numberProducts)> GetSupplierProductsForPageAsync(Guid supplierId, int pageNumber);
        Task<Guid> UpdateProductAsync(ProductDto product);
    }
}