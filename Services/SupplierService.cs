using Gvz.Laboratory.SupplierService.Abstractions;
using Gvz.Laboratory.SupplierService.Exceptions;
using Gvz.Laboratory.SupplierService.Models;

namespace Gvz.Laboratory.SupplierService.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly ISupplierMapper _supplierMapper;
        private readonly ISupplierKafkaProducer _supplierKafkaProducer;

        public SupplierService(ISupplierRepository supplierRepository, ISupplierMapper supplierMapper, ISupplierKafkaProducer supplierKafkaProducer)
        {
            _supplierRepository = supplierRepository;
            _supplierMapper = supplierMapper;
            _supplierKafkaProducer = supplierKafkaProducer;
        }

        public async Task<Guid> CreateSupplierAsync(Guid id, string name, List<Guid> manufacturersIds)
        {
            var (errors, supplier) = SupplierModel.Create(id, name);
            if (errors.Count > 0)
            {
                throw new SupplierValidationException(errors);
            }

            await _supplierRepository.CreateSupplierAsync(supplier, manufacturersIds);

            var supplierDto = _supplierMapper.MapTo(supplier) ?? throw new Exception("Mapping error");
            await _supplierKafkaProducer.SendToKafkaAsync(supplierDto, "add-supplier-topic");

            return id;
        }

        public async Task<List<SupplierModel>> GetSuppliersAsync()
        {
            return await _supplierRepository.GetSuppliersAsync();
        }

        public async Task<(List<SupplierModel> suppliers, int numberSuppliers)> GetSuppliersForPageAsync(int page)
        {
            return await _supplierRepository.GetSuppliersForPageAsync(page);
        }

        public async Task<Guid> UpdateSupplierAsync(Guid id, string name, List<Guid> manufacturersIds)
        {
            var (errors, supplier) = SupplierModel.Create(id, name);
            if (errors.Count > 0)
            {
                throw new SupplierValidationException(errors);
            }

            await _supplierRepository.UpdateSupplierAsync(supplier, manufacturersIds);

            var supplierDto = _supplierMapper.MapTo(supplier) ?? throw new Exception();
            await _supplierKafkaProducer.SendToKafkaAsync(supplierDto, "update-supplier-topic");

            return id;
        }

        public async Task DeleteSupplierAsync(List<Guid> ids)
        {
            await _supplierRepository.DeleteSupplierAsync(ids);

            await _supplierKafkaProducer.SendToKafkaAsync(ids, "delete-supplier-topic");
        }
    }
}
