using Gvz.Laboratory.SupplierService.Abstractions;
using Gvz.Laboratory.SupplierService.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gvz.Laboratory.SupplierService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager,Worker")]
        public async Task<ActionResult> CreateSupplierAsync([FromBody] CreateSupplierRequest createSupplierRequest)
        {
            var id = await _supplierService.CreateSupplierAsync(Guid.NewGuid(),
                                                                createSupplierRequest.SupplierName,
                                                                createSupplierRequest.ManufacturersIds);

            return Ok();
        }

        [HttpGet]
        [Route("getSuppliersAsync")]
        [Authorize]
        public async Task<ActionResult> GetSuppliersAsync()
        {
            var suppliers = await _supplierService.GetSuppliersAsync();

            var response = suppliers.Select(s => new GetSuppliersResponse(s.Id, s.SupplierName)).ToList();

            return Ok(response);
        }

        [HttpGet]
        [Route("getSuppliersForOptions")]
        [Authorize]
        public async Task<ActionResult> GetSuppliersForOptionsAsync()
        {
            var suppliers = await _supplierService.GetSuppliersAsync();

            var response = suppliers.Select(s => new GetSuppliersForOptionsResponse(s.Id, s.SupplierName, s.Manufacturers.Select(s => s.Id).ToList())).ToList();

            return Ok(response);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetSuppliersForPageAsync(int page)
        {
            var (suppliers, numberSuppliers) = await _supplierService.GetSuppliersForPageAsync(page);

            var response = suppliers.Select(s => new GetSuppliersResponse(s.Id, s.SupplierName)).ToList();

            var responseWrapper = new GetSuppliersForPageResponseWrapper(response, numberSuppliers);

            return Ok(responseWrapper);
        }

        [HttpGet]
        [Route("exportSuppliersToExcel")]
        [Authorize]
        public async Task<ActionResult> ExportSuppliersToExcelAsync()
        {
            var stream = await _supplierService.ExportSuppliersToExcelAsync();
            var fileName = "Suppliers.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet]
        [Route("searchSuppliers")]
        [Authorize]
        public async Task<ActionResult> SearchSuppliersAsync(string searchQuery, int pageNumber)
        {
            var (suppliers, numberSuppliers) = await _supplierService.SearchSuppliersAsync(searchQuery, pageNumber);
            var response = suppliers.Select(s => new GetSuppliersResponse(s.Id, s.SupplierName)).ToList();
            var responseWrapper = new GetSuppliersForPageResponseWrapper(response, numberSuppliers);
            return Ok(responseWrapper);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin,Manager,Worker")]
        public async Task<ActionResult> UpdateSupplierAsync(Guid id, [FromBody] UpdateSupplierRequest updateSupplierRequest)
        {
            await _supplierService.UpdateSupplierAsync(id, updateSupplierRequest.SupplierName, updateSupplierRequest.ManufacturersIds);
            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = "Admin,Manager,Worker")]
        public async Task<ActionResult> DeleteSuppliersAsync([FromBody] List<Guid> ids)
        {
            if (ids == null || !ids.Any())
            {
                return BadRequest("No supplier IDs provided.");
            }

            await _supplierService.DeleteSupplierAsync(ids);

            return Ok();
        }
    }
}
