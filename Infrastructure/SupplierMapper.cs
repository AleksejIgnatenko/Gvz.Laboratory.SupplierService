﻿using Gvz.Laboratory.SupplierService.Abstractions;
using Gvz.Laboratory.SupplierService.Dto;
using Gvz.Laboratory.SupplierService.Models;

namespace Gvz.Laboratory.SupplierService.Infrastructure
{
    public class SupplierMapper : ISupplierMapper
    {
        public SupplierDto? MapTo(SupplierModel supplier)
        {
            return supplier == null ? null : new SupplierDto()
            {
                Id = supplier.Id,
                Name = supplier.Name,
                Manufacturer = supplier.Manufacturer,
            };
        }
    }
}
