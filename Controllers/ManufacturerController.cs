using Gvz.Laboratory.SupplierService.Abstractions;
using Gvz.Laboratory.SupplierService.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gvz.Laboratory.SupplierService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManufacturerController : ControllerBase
    {
        private readonly IManufacturerService _manufacturerService;

        public ManufacturerController(IManufacturerService manufacturerService)
        {
            _manufacturerService = manufacturerService;
        }

        [HttpGet]
        [Route("getSupplierManufacturers")]
        [Authorize]
        public async Task<ActionResult> GetSupplierManufacturersAsync(Guid supplierId)
        {
            var manufacturers = await _manufacturerService.GetSupplierManufacturers(supplierId);

            var response = manufacturers.Select(m => new GetManufacturersResponse(m.Id, m.ManufacturerName)).ToList();

            return Ok(response);
        }

        [HttpGet]
        [Route("getSupplierManufacturersForPage")]
        [Authorize]
        public async Task<ActionResult> GetSupplierManufacturersForPageAsync(Guid supplierId, int pageNumber)
        {
            var (manufacturers, numberManufacturers) = await _manufacturerService.GetSupplierManufacturersForPageAsync(supplierId, pageNumber);

            var response = manufacturers.Select(m => new GetManufacturersResponse(m.Id, m.ManufacturerName)).ToList();

            var responseWrapper = new GetSupplierManufacturersForPageResponseWrapper(response, numberManufacturers);

            return Ok(responseWrapper);
        }
    }
}
