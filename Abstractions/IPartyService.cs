using Gvz.Laboratory.SupplierService.Models;

namespace Gvz.Laboratory.SupplierService.Abstractions
{
    public interface IPartyService
    {
        Task<(List<PartyModel> parties, int numberParties)> GetSupplierPartiesForPageAsync(Guid supplierId, int pageNumber);
    }
}