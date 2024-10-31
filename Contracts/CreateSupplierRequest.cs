namespace Gvz.Laboratory.SupplierService.Contracts
{
    public record CreateSupplierRequest(
        string SupplierName,
        List<Guid> ManufacturersIds
        );
}
