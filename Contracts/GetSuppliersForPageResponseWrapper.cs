namespace Gvz.Laboratory.SupplierService.Contracts
{
    public record GetSuppliersForPageResponseWrapper(
        List<GetSuppliersResponse> Suppliers,
        int NumberSuppliers
        );
}
