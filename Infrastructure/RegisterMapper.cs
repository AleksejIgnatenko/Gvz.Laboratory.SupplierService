using Gvz.Laboratory.SupplierService.Dto;
using Gvz.Laboratory.SupplierService.Models;
using Mapster;

namespace Gvz.Laboratory.SupplierService.Infrastructure
{
    public class RegisterMapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<SupplierModel, SupplierDto>()
                .RequireDestinationMemberSource(true);
        }
    }
}
