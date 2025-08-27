using AutoMapper;
using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.Billing.Core.Dto.Response;

namespace MoveEnergia.Billing.Core.MapperProfile
{
    public class TenantsProfile : Profile
    {
        public TenantsProfile() 
        {
            CreateMap<Tenants, TenantsReponseDto>().ReverseMap();

        }
    }
}
