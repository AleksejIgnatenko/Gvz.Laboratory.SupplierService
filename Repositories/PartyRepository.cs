using Gvz.Laboratory.SupplierService.Abstractions;
using Gvz.Laboratory.SupplierService.Dto;
using Gvz.Laboratory.SupplierService.Entities;
using Gvz.Laboratory.SupplierService.Models;
using Microsoft.EntityFrameworkCore;

namespace Gvz.Laboratory.SupplierService.Repositories
{
    public class PartyRepository : IPartyRepository
    {
        private readonly GvzLaboratorySupplierServiceDbContext _context;
        private readonly ISupplierRepository _supplierRepository;
        public PartyRepository(GvzLaboratorySupplierServiceDbContext context, ISupplierRepository supplierRepository)
        {
            _context = context;
            _supplierRepository = supplierRepository;
        }

        public async Task<Guid> CreatePartyAsync(PartyDto party)
        {
            var existingParty = await _context.Parties.FirstOrDefaultAsync(p => p.Id == party.Id);
            if (existingParty == null)
            {
                var supplierEntity = await _supplierRepository.GetSupplierEntityByIdAsync(party.SupplierId)
                    ?? throw new InvalidOperationException($"Supplier with Id '{party.SupplierId}' was not found.");

                var partyEntity = new PartyEntity
                {
                    Id = party.Id,
                    BatchNumber = party.BatchNumber,
                    DateOfReceipt = party.DateOfReceipt,
                    ProductName = party.ProductName,
                    Supplier = supplierEntity,
                    ManufacturerName = party.ManufacturerName,
                    BatchSize = party.BatchSize,
                    SampleSize = party.SampleSize,
                    TTN = party.TTN,
                    DocumentOnQualityAndSafety = party.DocumentOnQualityAndSafety,
                    TestReport = party.TestReport,
                    DateOfManufacture = party.DateOfManufacture,
                    ExpirationDate = party.ExpirationDate,
                    Packaging = party.Packaging,
                    Marking = party.Marking,
                    Result = party.Result,
                    Note = party.Note,
                    Surname = party.Surname,
                };
                await _context.Parties.AddAsync(partyEntity);
                await _context.SaveChangesAsync();
            }
            return party.Id;
        }

        public async Task<(List<PartyModel> parties, int numberParties)> GetSupplierPartiesForPageAsync(Guid supplierId, int pageNumber)
        {
            var partyEntities = await _context.Parties
                    .AsNoTracking()
                    .Where(p => p.Supplier.Id == supplierId)
                    .Include(p => p.Supplier)
                    .Skip(pageNumber * 20)
                    .Take(20)
                    .ToListAsync();

            if (!partyEntities.Any() && pageNumber != 0)
            {
                pageNumber--;
                partyEntities = await _context.Parties
                    .AsNoTracking()
                    .Where(p => p.Supplier.Id == supplierId)
                    .Include(p => p.Supplier)
                    .Skip(pageNumber * 20)
                    .Take(20)
                    .ToListAsync();
            }

            var numberParties = await _context.Parties
                .Where(p => p.Supplier.Id == supplierId)
                .CountAsync();

            var parties = partyEntities.Select(p => PartyModel.Create(
                p.Id,
                p.BatchNumber,
                p.DateOfReceipt,
                p.ProductName,
                SupplierModel.Create(p.Supplier.Id, p.Supplier.SupplierName, false).supplier,
                p.ManufacturerName,
                p.BatchSize,
                p.SampleSize,
                p.TTN,
                p.DocumentOnQualityAndSafety,
                p.TestReport,
                p.DateOfManufacture,
                p.ExpirationDate,
                p.Packaging,
                p.Marking,
                p.Result,
                p.Surname,
                p.Note
                )).ToList();

            return (parties, numberParties);
        }

        public async Task<Guid> UpdatePartyAsync(PartyDto party)
        {
            var existingParty = await _context.Parties.FirstOrDefaultAsync(p => p.Id == party.Id)
                ?? throw new InvalidOperationException($"Party with Id '{party.Id}' was not found.");

            var supplierEntity = await _supplierRepository.GetSupplierEntityByIdAsync(party.SupplierId)
                ?? throw new InvalidOperationException($"Supplier with Id '{party.SupplierId}' was not found.");

            existingParty.BatchNumber = party.BatchNumber;
            existingParty.DateOfReceipt = party.DateOfReceipt;
            existingParty.ProductName = party.ProductName;
            existingParty.Supplier = supplierEntity;
            existingParty.ManufacturerName = party.ManufacturerName;
            existingParty.BatchSize = party.BatchSize;
            existingParty.SampleSize = party.SampleSize;
            existingParty.TTN = party.TTN;
            existingParty.DocumentOnQualityAndSafety = party.DocumentOnQualityAndSafety;
            existingParty.TestReport = party.TestReport;
            existingParty.DateOfManufacture = party.DateOfManufacture;
            existingParty.ExpirationDate = party.ExpirationDate;
            existingParty.Packaging = party.Packaging;
            existingParty.Marking = party.Marking;
            existingParty.Result = party.Result;
            existingParty.Note = party.Note;

            await _context.SaveChangesAsync();

            return party.Id;
        }

        public async Task DeletePartiesAsync(List<Guid> ids)
        {
            await _context.Parties
                .Where(s => ids.Contains(s.Id))
                .ExecuteDeleteAsync();
        }
    }
}
