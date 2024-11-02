namespace Gvz.Laboratory.SupplierService.Contracts
{
    public record GetSupplierManufacturersForPageResponseWrapper(
        List<GetManufacturersResponse> Manufacturers,
        int numberManufacturers
        );
}
