﻿namespace Gvz.Laboratory.SupplierService.Dto
{
    public class SupplierDto
    {
        public Guid Id { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public List<Guid> SupplierIds { get; set; } = new List<Guid>();
    }
}
