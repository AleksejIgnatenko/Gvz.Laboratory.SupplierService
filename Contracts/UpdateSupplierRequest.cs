namespace Gvz.Laboratory.SupplierService.Contracts
{
    public record UpdateSupplierRequest(
        Guid Id,
        string SupplierName
        );
}
