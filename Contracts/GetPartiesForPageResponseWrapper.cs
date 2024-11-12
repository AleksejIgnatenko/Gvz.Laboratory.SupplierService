namespace Gvz.Laboratory.SupplierService.Contracts
{
    public record GetPartiesForPageResponseWrapper(
            List<GetPartiesResponse> Parties,
            int numberParties
            );
}
