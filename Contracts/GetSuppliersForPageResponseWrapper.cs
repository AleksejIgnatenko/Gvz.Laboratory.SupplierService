namespace Gvz.Laboratory.SupplierService.Contracts
{
    public record GetSuppliersForPageResponseWrapper(
        List<GetSuppliersForPageResponse> Suppliers,
        int NumberUsers
        );
}
