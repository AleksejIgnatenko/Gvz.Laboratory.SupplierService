﻿using Gvz.Laboratory.SupplierService.Entities;

namespace Gvz.Laboratory.SupplierService.Dto
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string UnitsOfMeasurement { get; set; } = string.Empty;
        public List<Guid> SupplierIds { get; set; } = new List<Guid>();
    }
}
