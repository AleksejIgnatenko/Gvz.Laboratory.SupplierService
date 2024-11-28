namespace Gvz.Laboratory.SupplierService.Models
{
    public class ProductModel
    {
        public Guid Id { get; }
        public string ProductName { get; } = string.Empty;
        public string UnitsOfMeasurement { get; } = string.Empty;
        //public List<SupplierModel> Suppliers { get; } = new List<SupplierModel>();

        public ProductModel(Guid id, string productName, string unitsOfMeasurement)
        {
            Id = id;
            ProductName = productName;
            UnitsOfMeasurement = unitsOfMeasurement;
        }

        public static ProductModel Create(Guid id, string productName, string unitsOfMeasurement)
        {
            return new ProductModel(id, productName, unitsOfMeasurement);
        }
    }
}
