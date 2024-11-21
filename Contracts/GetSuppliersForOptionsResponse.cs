namespace Gvz.Laboratory.SupplierService.Contracts
{
    public record GetSuppliersForOptionsResponse(
        Guid Id,
        string SupplierName,
        List<Guid> ManufacturersIds
        );
}
