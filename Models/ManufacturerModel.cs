namespace Gvz.Laboratory.SupplierService.Models
{
    public class ManufacturerModel
    {
        public Guid Id { get; }
        public string ManufacturerName { get; } = string.Empty;

        public ManufacturerModel(Guid id, string manufacturerName)
        {
            Id = id;
            ManufacturerName = manufacturerName;
        }

        public static ManufacturerModel Create(Guid id, string manufacturerName, bool useValidation = true)
        {
            ManufacturerModel manufacturer = new ManufacturerModel(id, manufacturerName);
            return manufacturer;
        }
    }
}
