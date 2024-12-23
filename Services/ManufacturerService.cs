﻿using Gvz.Laboratory.SupplierService.Abstractions;
using Gvz.Laboratory.SupplierService.Models;

namespace Gvz.Laboratory.SupplierService.Services
{
    public class ManufacturerService : IManufacturerService
    {
        private readonly IManufacturerRepository _manufacturerRepository;

        public ManufacturerService(IManufacturerRepository manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }

        public async Task<(List<ManufacturerModel> manufacturers, int numberManufacturers)> GetSupplierManufacturersForPageAsync(Guid supplierId, int pageNumber)
        {
            return await _manufacturerRepository.GetSupplierManufacturersForPageAsync(supplierId, pageNumber);
        }

        public async Task<List<ManufacturerModel>> GetSupplierManufacturers(Guid supplierId)
        {
            return await _manufacturerRepository.GetSupplierManufacturersAsync(supplierId);
        }
    }
}
