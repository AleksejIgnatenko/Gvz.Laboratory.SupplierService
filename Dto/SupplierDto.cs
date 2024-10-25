namespace Gvz.Laboratory.SupplierService.Dto
{
    public class SupplierDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public DateTime DateCreate { get; set; }
    }
}
