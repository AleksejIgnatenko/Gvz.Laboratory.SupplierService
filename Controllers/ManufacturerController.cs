using Gvz.Laboratory.SupplierService.Abstractions;
using Gvz.Laboratory.SupplierService.Contracts;
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
        public async Task<ActionResult> GetSupplierManufacturers(Guid supplierId)
        {
            var manufacturers = await _manufacturerService.GetSupplierManufacturers(supplierId);

            var response = manufacturers.Select(m => new GetManufacturersResponse(m.Id, m.ManufacturerName)).ToList();

            return Ok(response);
        }
    }
}
