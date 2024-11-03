namespace Gvz.Laboratory.SupplierService.Models
{
    public class ProductModel
    {
        public Guid Id { get; }
        public string ProductName { get; } = string.Empty;
        public List<SupplierModel> Suppliers { get; } = new List<SupplierModel>();

        public ProductModel(Guid id, string productName)
        {
            Id = id;
            ProductName = productName;
        }

        public static ProductModel Create(Guid id, string productName)
        {
            ProductModel product = new ProductModel(id, productName);
            return product;
        }
    }
}
