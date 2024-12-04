using Gvz.Laboratory.SupplierService.Abstractions;
using Gvz.Laboratory.SupplierService.Contracts;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<ActionResult> GetSupplierProductsForPageAsync(Guid supplierId, int pageNumber)
        {
            var (products, numberSuppliers) = await _productService.GetSupplierProductsForPageAsync(supplierId, pageNumber);
            var response = products.Select(p => new GetProductsResponse(p.Id, p.ProductName, p.UnitsOfMeasurement)).ToList();
            var responseWrapper = new GetProductsSuppliersForPageResponseWrapper(response, numberSuppliers);
            return Ok(responseWrapper);
        }
    }
}
