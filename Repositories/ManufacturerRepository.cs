using Gvz.Laboratory.SupplierService.Abstractions;
using Gvz.Laboratory.SupplierService.Dto;
using Gvz.Laboratory.SupplierService.Entities;
using Gvz.Laboratory.SupplierService.Models;
using Microsoft.EntityFrameworkCore;

namespace Gvz.Laboratory.SupplierService.Repositories
{
    public class ManufacturerRepository : IManufacturerRepository
    {
        private readonly GvzLaboratorySupplierServiceDbContext _context;

        public ManufacturerRepository(GvzLaboratorySupplierServiceDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateManufacturerAsync(ManufacturerDto manufacturer)
        {
            var existingManufacturer = await _context.Manufacturers.FirstOrDefaultAsync(m => m.ManufacturerName.Equals(manufacturer.ManufacturerName));

            if (existingManufacturer == null)
            {

                var manufacturerEntity = new ManufacturerEntity
                {
                    Id = manufacturer.Id,
                    ManufacturerName = manufacturer.ManufacturerName,
                };

                await _context.Manufacturers.AddAsync(manufacturerEntity);
                await _context.SaveChangesAsync();
            }

            return manufacturer.Id;
        }

        public async Task<List<ManufacturerModel>> GetSupplierManufacturersAsync(Guid supplierId)
        {
            var manufacturerEntities = await _context.Manufacturers
                .Where(m => m.Supplier.Id == supplierId).ToListAsync();

            var manufacturers = manufacturerEntities.Select(m => ManufacturerModel.Create(m.Id, m.ManufacturerName)).ToList();

            return manufacturers;
        }

        public async Task<(List<ManufacturerModel> manufacturers, int numberManufacturers)> GetSupplierManufacturersForPageAsync(Guid supplierId, int pageNumber)
        {
            var manufacturerEntities = await _context.Manufacturers
                .AsNoTracking()
                .Where(m => m.Supplier.Id == supplierId)
                .Skip(pageNumber * 20)
                .Take(20)
                .ToListAsync();

            var numberManufacturers = await _context.Manufacturers
                .Where(m => m.Supplier.Id == supplierId)
                .CountAsync();

            var manufacturers = manufacturerEntities.Select(m => ManufacturerModel.Create(m.Id, m.ManufacturerName)).ToList();

            return (manufacturers, numberManufacturers);
        }

        public async Task<List<ManufacturerEntity>> GetManufacturersByIdsAsync(List<Guid> manufacturersIds)
        {
            var manufacturerEntities = await _context.Manufacturers
                .Where(m => manufacturersIds.Contains(m.Id)).ToListAsync();

            return manufacturerEntities;
        }

        public async Task<Guid> UpdateManufacturerAsync(ManufacturerDto manufacturer)
        {
            await _context.Manufacturers
                .Where(m => m.Id == manufacturer.Id)
                .ExecuteUpdateAsync(m => m
                    .SetProperty(m => m.ManufacturerName, manufacturer.ManufacturerName)
                 );

            return manufacturer.Id;
        }

        public async Task DeleteManufacturersAsync(List<Guid> ids)
        {
            await _context.Manufacturers
                .Where(s => ids.Contains(s.Id))
                .ExecuteDeleteAsync();
        }
    }
}
