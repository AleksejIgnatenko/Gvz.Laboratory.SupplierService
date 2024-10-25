using FluentValidation.Results;
using Gvz.Laboratory.SupplierService.Validations;

namespace Gvz.Laboratory.SupplierService.Models
{
    public class SupplierModel
    {
        public Guid Id { get; }
        public string Name { get; } = string.Empty;
        public string Manufacturer { get; } = string.Empty;

        public SupplierModel(Guid id, string name, string manufacturer)
        {
            Id = id;
            Name = name;
            Manufacturer = manufacturer;
        }

        public static (Dictionary<string, string> errors, SupplierModel supplier) Create(Guid id, string name, string manufacturer, 
            bool useValidation = true)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            SupplierModel supplier = new SupplierModel(id, name, manufacturer);
            if(!useValidation) { return (errors, supplier); }

            SupplierValidation supplierValidation = new SupplierValidation();
            ValidationResult validationResult = supplierValidation.Validate(supplier);
            if(!validationResult.IsValid)
            {
                foreach(var failure in validationResult.Errors)
                {
                    errors[failure.PropertyName] = failure.ErrorMessage;
                }
            }

            return (errors, supplier);
        }
    }
}
