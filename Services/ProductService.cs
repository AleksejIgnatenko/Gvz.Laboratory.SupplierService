using Gvz.Laboratory.SupplierService.Abstractions;
using Gvz.Laboratory.SupplierService.Models;

namespace Gvz.Laboratory.SupplierService.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<(List<ProductModel> suppliers, int numberSuppliers)> GetSupplierProductsForPageAsync(Guid supplierId, int pageNumber)
        {
            return await _productRepository.GetSupplierProductsForPageAsync(supplierId, pageNumber);
        }
    }
}
