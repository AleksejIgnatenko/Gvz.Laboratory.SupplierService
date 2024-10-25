using Gvz.Laboratory.SupplierService.Abstractions;
using Gvz.Laboratory.SupplierService.Entities;
using Gvz.Laboratory.SupplierService.Exceptions;
using Gvz.Laboratory.SupplierService.Models;
using Microsoft.EntityFrameworkCore;

namespace Gvz.Laboratory.SupplierService.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly GvzLaboratorySupplierServiceDbContext _context;

        public SupplierRepository(GvzLaboratorySupplierServiceDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateSupplierAsync(SupplierModel supplier)
        {
            var existingSupplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.Name.Equals(supplier.Name));

            if (existingSupplier != null) { throw new RepositoryException("Такой поставщик уже существует"); }

            var supplierEntity = new SupplierEntity
            {
                Id = supplier.Id,
                Name = supplier.Name,
                Manufacturer = supplier.Manufacturer,
                DateCreate = DateTime.UtcNow,
            };

            await _context.Suppliers.AddAsync(supplierEntity);
            await _context.SaveChangesAsync();

            return supplierEntity.Id;
        }

        public async Task<(List<SupplierModel> suppliers, int numberSuppliers)> GetSuppliersForPageAsync(int page)
        {
            var supplierEntities = await _context.Suppliers
                .AsNoTracking()
                .OrderByDescending(s => s.DateCreate)
                .Skip(page * 20)
                .Take(20)
                .ToListAsync();

            var numberSuppliers = await _context.Suppliers.CountAsync();

            var suppliers = supplierEntities.Select(s => SupplierModel.Create(
                s.Id,
                s.Name,
                s.Manufacturer,
                false).supplier).ToList();

            return (suppliers, numberSuppliers);
        }

        public async Task<Guid> UpdateSupplierAsync(SupplierModel supplier)
        {
            await _context.Suppliers
                .Where(s => s.Id == supplier.Id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(s => s.Name, supplier.Name)
                    .SetProperty(s => s.Manufacturer, supplier.Manufacturer)
                );

            return supplier.Id;
        }

        public async Task<Guid> DeleteSupplierAsync(Guid id)
        {
            await _context.Suppliers
                .Where(s => s.Id == id)
                .ExecuteDeleteAsync();

            return id;
        }
    }
}
