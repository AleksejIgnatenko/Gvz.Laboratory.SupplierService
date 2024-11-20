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

            var manufacturerEntities = await _manufacturerRepository.GetManufacturersByIdsAsync(manufacturersIds) 
                ?? throw new RepositoryException("Производитель(и) не был(и) найден(ы)");

            var supplierEntity = new SupplierEntity
            {
                Id = supplier.Id,
                SupplierName = supplier.SupplierName,
                Manufacturers = manufacturerEntities,
                DateCreate = DateTime.UtcNow,
            };

            await _context.Suppliers.AddAsync(supplierEntity);
            await _context.SaveChangesAsync();

            return supplierEntity.Id;
        }

        public async Task<List<SupplierModel>> GetSuppliersAsync()
        {
            var supplierEntities = await _context.Suppliers
                .AsNoTracking()
                .OrderByDescending(s => s.DateCreate)
                .ToListAsync();

            var suppliers = supplierEntities.Select(s => SupplierModel.Create(
                s.Id, 
                s.SupplierName, 
                false).supplier).ToList();

            return suppliers;
        }

        public async Task<SupplierEntity?> GetSupplierEntityByIdAsync(Guid supplierId)
        {
            return await _context.Suppliers.FirstOrDefaultAsync(s => s.Id == supplierId);
        }

        public async Task<List<SupplierEntity>> GetSuppliersByIdsAsync(List<Guid> suppliersIds)
        {
            var supplierEntities = await _context.Suppliers
                .Where(s => suppliersIds.Contains(s.Id)).ToListAsync();

            return supplierEntities;
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

        public async Task<Guid> UpdateSupplierAsync(SupplierModel supplier, List<Guid> manufacturersIds)
        {
            var manufacturerEntities = await _manufacturerRepository.GetManufacturersByIdsAsync(manufacturersIds)
                ?? throw new RepositoryException("Производитель(и) не был(и) найден(ы)");

            var existingSupplier = await _context.Suppliers
                .Include(s => s.Manufacturers)
                .FirstOrDefaultAsync(s => s.Id == supplier.Id)
                ?? throw new RepositoryException("Поставщик не найден");

            var existingSupplierName = await _context.Suppliers
                .FirstOrDefaultAsync(s => (s.SupplierName == supplier.SupplierName) && (s.SupplierName != existingSupplier.SupplierName));
            if (existingSupplierName != null)
            {
                throw new RepositoryException("Поставщик с таким именем уже существует.");
            }

            existingSupplier.SupplierName = supplier.SupplierName;

            existingSupplier.Manufacturers.Clear();
            existingSupplier.Manufacturers.AddRange(manufacturerEntities);

            await _context.SaveChangesAsync();

            return existingSupplier.Id;
        }

        public async Task DeleteSupplierAsync(List<Guid> ids)
        {
            var supplierEntities = await _context.Suppliers
                .Include(s => s.Manufacturers)
                .Where(s => ids.Contains(s.Id))
                .ToListAsync();

            foreach (var supplierEntity in supplierEntities)
            {
                supplierEntity.Manufacturers.Clear();
            }

            await _context.SaveChangesAsync();

            await _context.Suppliers
                .Where(s => ids.Contains(s.Id))
                .ExecuteDeleteAsync();
        }
    }
}
