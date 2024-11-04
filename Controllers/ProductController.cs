using Gvz.Laboratory.SupplierService.Abstractions;
using Gvz.Laboratory.SupplierService.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Gvz.Laboratory.SupplierService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController  : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route("getSupplierProductsForPageAsync")]
        public async Task<ActionResult> GetSupplierProductsForPageAsync(Guid supplierId, int pageNumber)
        {
            var (products, numberSuppliers) = await _productService.GetSupplierProductsForPageAsync(supplierId, pageNumber);
            var response = products.Select(s => new GetProductsResponse(s.Id, s.ProductName)).ToList();
            var responseWrapper = new GetProductsSuppliersForPageResponseWrapper(response, numberSuppliers);
            return Ok(responseWrapper);
        }
    }
}
