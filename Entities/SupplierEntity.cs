namespace Gvz.Laboratory.SupplierService.Entities
{
    public class SupplierEntity
    {
        public Guid Id { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public List<ManufacturerEntity> Manufacturers { get; set; } = new List<ManufacturerEntity>();
        public DateTime DateCreate { get; set; }
    }
}
