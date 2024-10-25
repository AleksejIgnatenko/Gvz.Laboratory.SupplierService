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

        public async Task<Guid> CreateSupplierAsync(Guid id, string name, string manufacturer)
        {
            var (errors, supplier) = SupplierModel.Create(id, name, manufacturer);
            if (errors.Count > 0)
            {
                throw new SupplierValidationException(errors);
            }

            await _supplierRepository.CreateSupplierAsync(supplier);

            var supplierDto = _supplierMapper.MapTo(supplier) ?? throw new Exception();
            await _supplierKafkaProducer.SendUserToKafka(supplierDto, "add-supplier-topic");

            return id;
        }

        public async Task<(List<SupplierModel> suppliers, int numberSuppliers)> GetSuppliersForPageAsync(int page)
        {
            return await _supplierRepository.GetSuppliersForPageAsync(page);
        }

        public async Task<Guid> UpdateSupplierAsync(Guid id, string name, string manufacturer)
        {
            var (errors, supplier) = SupplierModel.Create(id, name, manufacturer);
            if (errors.Count > 0)
            {
                throw new SupplierValidationException(errors);
            }

            await _supplierRepository.UpdateSupplierAsync(supplier);

            var supplierDto = _supplierMapper.MapTo(supplier) ?? throw new Exception();
            await _supplierKafkaProducer.SendUserToKafka(supplierDto, "update-supplier-topic");

            return id;
        }

        public async Task<Guid> DeleteSupplierAsync(Guid id)
        {
            await _supplierRepository.DeleteSupplierAsync(id);

            await _supplierKafkaProducer.SendUserToKafka(id, "delete-supplier-topic");

            return id;
        }
    }
}
