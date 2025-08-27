using AutoMapper;
using MoveEnergia.Billing.Core.Dto.Request;
using MoveEnergia.Billing.Core.Entity;

namespace MoveEnergia.Billing.Core.MapperProfile
{
    public class DistributorProfile : Profile
    {
        public DistributorProfile()
        {
            CreateMap<Distributor, DistributorRequestDto>().ReverseMap();

        }
    }
}
    