using Gvz.Laboratory.SupplierService.Abstractions;
using Gvz.Laboratory.SupplierService.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gvz.Laboratory.SupplierService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartyController : ControllerBase
    {
        private readonly IPartyService _partyService;
        public PartyController(IPartyService partyService)
        {
            _partyService = partyService;
        }

        [HttpGet]
        [Route("getSupplierPartiesForPage")]        
        [Authorize]
        public async Task<ActionResult> GetSupplierPartiesForPageAsync(Guid supplierId, int pageNumber)
        {
            var (parties, numberParties) = await _partyService.GetSupplierPartiesForPageAsync(supplierId, pageNumber);
            var response = parties.Select(p => new GetPartiesResponse(p.Id,
                p.BatchNumber,
                p.DateOfManufacture,
                p.ProductName,
                p.Supplier.SupplierName,
                p.ManufacturerName,
                p.BatchSize,
                p.SampleSize,
                p.TTN,
                p.DocumentOnQualityAndSafety,
                p.TestReport,
                p.DateOfManufacture,
                p.ExpirationDate,
                p.Packaging,
                p.Marking,
                p.Result,
                p.Surname,
                p.Note)).ToList();

            var responseWrapper = new GetPartiesForPageResponseWrapper(response, numberParties);

            return Ok(responseWrapper);
        }
    }
}
