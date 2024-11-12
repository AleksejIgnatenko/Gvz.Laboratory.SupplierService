using Gvz.Laboratory.SupplierService.Abstractions;
using Gvz.Laboratory.SupplierService.Models;

namespace Gvz.Laboratory.SupplierService.Services
{
    public class PartyService : IPartyService
    {
        private readonly IPartyRepository _partyRepository;
        public PartyService(IPartyRepository partyRepository)
        {
            _partyRepository = partyRepository;
        }
        public async Task<(List<PartyModel> parties, int numberParties)> GetSupplierPartiesForPageAsync(Guid supplierId, int pageNumber)
        {
            return await _partyRepository.GetSupplierPartiesForPageAsync(supplierId, pageNumber);
        }
    }
}
