namespace Gvz.Laboratory.SupplierService.Entities
{
    public class SupplierEntity
    {
        public Guid Id { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public List<ManufacturerEntity> Manufacturers { get; set; } = new List<ManufacturerEntity>();
        public List<ProductEntity> Products { get; set; } = new List<ProductEntity>();
        public DateTime DateCreate { get; set; }
    }
}
