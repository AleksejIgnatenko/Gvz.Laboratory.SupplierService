using Gvz.Laboratory.SupplierService.Abstractions;
using Gvz.Laboratory.SupplierService.Exceptions;
using Gvz.Laboratory.SupplierService.Models;
using OfficeOpenXml;

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

        public async Task<(List<SupplierModel> suppliers, int numberSuppliers)> SearchSuppliersAsync(string searchQuery, int pageNumber)
        {
            return await _supplierRepository.SearchSuppliersAsync(searchQuery, pageNumber);
        }

        public async Task<MemoryStream> ExportSuppliersToExcelAsync()
        {
            var manufacturers = await _supplierRepository.GetSuppliersAsync();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Suppliers");

                worksheet.Cells[1, 1].Value = "Id";
                worksheet.Cells[1, 2].Value = "Название";

                for (int i = 0; i < manufacturers.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = manufacturers[i].Id;
                    worksheet.Cells[i + 2, 2].Value = manufacturers[i].SupplierName;
                }

                worksheet.Cells.AutoFitColumns();

                var stream = new MemoryStream();
                await package.SaveAsAsync(stream);

                stream.Position = 0; // Сбрасываем поток
                return stream;
            }
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
