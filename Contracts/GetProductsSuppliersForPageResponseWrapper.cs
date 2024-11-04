namespace Gvz.Laboratory.SupplierService.Contracts
{
    public record GetProductsSuppliersForPageResponseWrapper(
        List<GetProductsResponse> Products,
        int numberProducts
        );
}
