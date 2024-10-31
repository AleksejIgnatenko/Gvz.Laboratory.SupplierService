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
        private readonly IManufacturerRepository _manufacturerRepository;

        public SupplierRepository(GvzLaboratorySupplierServiceDbContext context, IManufacturerRepository manufacturerRepository)
        {
            _context = context;
            _manufacturerRepository = manufacturerRepository;
        }

        public async Task<Guid> CreateSupplierAsync(SupplierModel supplier, List<Guid> manufacturersIds)
        {
            var existingSupplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.SupplierName.Equals(supplier.SupplierName));

            if (existingSupplier != null) { throw new RepositoryException("Такой поставщик уже существует"); }

            var manufacturers = await _manufacturerRepository.GetManufacturersByIds(manufacturersIds) 
                ?? throw new RepositoryException("Поставщики не были найдены");

            var supplierEntity = new SupplierEntity
            {
                Id = supplier.Id,
                SupplierName = supplier.SupplierName,
                Manufacturers = manufacturers,
                DateCreate = DateTime.UtcNow,
            };

            await _context.Suppliers.AddAsync(supplierEntity);
            await _context.SaveChangesAsync();

            return supplierEntity.Id;
        }

        public async Task<(List<SupplierModel> suppliers, int numberSuppliers)> GetSuppliersForPageAsync(int pageNumber)
        {
            var supplierEntities = await _context.Suppliers
                .AsNoTracking()
                .OrderByDescending(s => s.DateCreate)
                .Skip(pageNumber * 20)
                .Take(20)
                .ToListAsync();

            if (!supplierEntities.Any() && pageNumber != 0)
            {
                pageNumber--;
                supplierEntities = await _context.Suppliers
                    .AsNoTracking()
                    .OrderByDescending(s => s.DateCreate)
                    .Skip(pageNumber * 20)
                    .Take(20)
                    .ToListAsync();
            }

            var numberSuppliers = await _context.Suppliers.CountAsync();

            var suppliers = supplierEntities.Select(s => SupplierModel.Create(
                s.Id,
                s.SupplierName,
                false).supplier).ToList();

            return (suppliers, numberSuppliers);
        }

        public async Task<Guid> UpdateSupplierAsync(SupplierModel supplier)
        {
            await _context.Suppliers
                .Where(s => s.Id == supplier.Id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(s => s.SupplierName, supplier.SupplierName)
                );

            return supplier.Id;
        }

        public async Task DeleteSupplierAsync(List<Guid> ids)
        {
            await _context.Suppliers
                .Where(s => ids.Contains(s.Id))
                .ExecuteDeleteAsync();
        }
    }
}
