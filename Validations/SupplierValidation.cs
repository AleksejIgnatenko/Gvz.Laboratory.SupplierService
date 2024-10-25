using FluentValidation;
using Gvz.Laboratory.SupplierService.Models;

namespace Gvz.Laboratory.SupplierService.Validations
{
    public class SupplierValidation : AbstractValidator<SupplierModel>
    {
        public SupplierValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Фамилия не может быть пустая");

            RuleFor(x => x.Manufacturer)
                .NotEmpty().WithMessage("Фамилия не может быть пустая");
        }
    }
}
