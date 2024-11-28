﻿namespace Gvz.Laboratory.SupplierService.Entities
{
    public class ProductEntity
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string UnitsOfMeasurement { get; set; } = string.Empty;
        public List<SupplierEntity> Suppliers { get; set; } = new List<SupplierEntity>();
    }
}
