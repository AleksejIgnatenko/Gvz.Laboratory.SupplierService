using Gvz.Laboratory.SupplierService.Dto;
using Gvz.Laboratory.SupplierService.Models;
using Mapster;

namespace Gvz.Laboratory.SupplierService.Abstractions
{
    [Mapper]
    public interface ISupplierMapper
    {
        SupplierDto? MapTo(SupplierModel supplier);
    }
}