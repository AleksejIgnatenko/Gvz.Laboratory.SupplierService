namespace Gvz.Laboratory.SupplierService.Contracts
{
    public record GetSuppliersForPageResponse(
        Guid Id,
        string Name,
        string Manufacturer
        );
}
