using AutoMapper;
using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.Billing.Core.Dto.Response;

namespace MoveEnergia.Billing.Core.MapperProfile
{
    public class ConsumerUnitProfile : Profile
    {
        public ConsumerUnitProfile()
        {
            CreateMap<ConsumerUnit, CustomerUnitResponseDto>().ReverseMap();

        }
    }
}
