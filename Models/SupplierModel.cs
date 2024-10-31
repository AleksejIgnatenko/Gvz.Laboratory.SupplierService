using FluentValidation.Results;
using Gvz.Laboratory.SupplierService.Validations;

namespace Gvz.Laboratory.SupplierService.Models
{
    public class SupplierModel
    {
        public Guid Id { get; }
        public string SupplierName { get; } = string.Empty;
        public List<ManufacturerModel> Manufacturers { get; } = new List<ManufacturerModel>();

        public SupplierModel(Guid id, string name)
        {
            Id = id;
            SupplierName = name;
        }

        public SupplierModel(Guid id, string name, List<ManufacturerModel> manufacturers)
        {
            Id = id;
            SupplierName = name;
            Manufacturers = manufacturers;
        }

        public static (Dictionary<string, string> errors, SupplierModel supplier) Create(Guid id, string name, bool useValidation = true)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            SupplierModel supplier = new SupplierModel(id, name);
            if (!useValidation) { return (errors, supplier); }

            SupplierValidation supplierValidation = new SupplierValidation();
            ValidationResult validationResult = supplierValidation.Validate(supplier);
            if (!validationResult.IsValid)
            {
                foreach (var failure in validationResult.Errors)
                {
                    errors[failure.PropertyName] = failure.ErrorMessage;
                }
            }

            return (errors, supplier);
        }

        public static (Dictionary<string, string> errors, SupplierModel supplier) Create(Guid id, string name,
            List<ManufacturerModel> manufacturers, bool useValidation = true)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            SupplierModel supplier = new SupplierModel(id, name, manufacturers);
            if (!useValidation) { return (errors, supplier); }

            SupplierValidation supplierValidation = new SupplierValidation();
            ValidationResult validationResult = supplierValidation.Validate(supplier);
            if (!validationResult.IsValid)
            {
                foreach (var failure in validationResult.Errors)
                {
                    errors[failure.PropertyName] = failure.ErrorMessage;
                }
            }

            return (errors, supplier);
        }
    }
}
