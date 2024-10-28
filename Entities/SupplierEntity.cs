namespace Gvz.Laboratory.SupplierService.Entities
{
    public class SupplierEntity
    {
        public Guid Id { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public DateTime DateCreate { get; set; }
    }
}
