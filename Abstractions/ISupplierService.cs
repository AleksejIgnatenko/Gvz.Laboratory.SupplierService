﻿using Gvz.Laboratory.SupplierService.Models;

namespace Gvz.Laboratory.SupplierService.Abstractions
{
    public interface ISupplierService
    {
        Task<Guid> CreateSupplierAsync(Guid id, string name, string manufacturer);
        Task<(List<SupplierModel> suppliers, int numberSuppliers)> GetSuppliersForPageAsync(int page);
        Task<Guid> DeleteSupplierAsync(Guid id);
        Task<Guid> UpdateSupplierAsync(Guid id, string name, string manufacturer);
    }
}