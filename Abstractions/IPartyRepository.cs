using Gvz.Laboratory.SupplierService.Dto;
using Gvz.Laboratory.SupplierService.Models;

namespace Gvz.Laboratory.SupplierService.Abstractions
{
    public interface IPartyRepository
    {
        Task<Guid> CreatePartyAsync(PartyDto party);
        Task DeletePartiesAsync(List<Guid> ids);
        Task<(List<PartyModel> parties, int numberParties)> GetSupplierPartiesForPageAsync(Guid supplierId, int pageNumber);
        Task<Guid> UpdatePartyAsync(PartyDto party);
    }
}