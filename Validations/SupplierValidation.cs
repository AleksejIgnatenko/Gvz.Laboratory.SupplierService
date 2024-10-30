using FluentValidation;
using Gvz.Laboratory.SupplierService.Models;

namespace Gvz.Laboratory.SupplierService.Validations
{
    public class SupplierValidation : AbstractValidator<SupplierModel>
    {
        public SupplierValidation()
        {
            RuleFor(x => x.SupplierName)
                .NotEmpty().WithMessage("Название поставщика не может быть пустым");

            RuleFor(x => x.Manufacturer)
                .NotEmpty().WithMessage("Название производителя не может быть пустым");
        }
    }
}
