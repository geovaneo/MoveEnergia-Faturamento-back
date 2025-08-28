using AutoMapper;
using Microsoft.Extensions.Logging;
using MoveEnergia.RdStation.Adapter.Interface.Adapter;
using MoveEnergia.RdStation.Adapter.Interface.Service;

namespace MoveEnergia.RdStation.Adapter
{
    public class RdstationIntegrationAdapter : IRdstationIntegrationAdapter
    {
        private readonly IMapper _mapper;
        private readonly ILogger<RdstationIntegrationAdapter> _logger;
        private readonly IRdstationIntegrationService _iRdstationIntegrationService;
        public RdstationIntegrationAdapter(ILogger<RdstationIntegrationAdapter> logger,
                                           IMapper mapper,
                                           IRdstationIntegrationService iRdstationIntegrationService)
        {
            _logger = logger;
            _mapper = mapper;
            _iRdstationIntegrationService = iRdstationIntegrationService;
        }

        public async Task GetCellphoneNumbersAsync(string dealId, string token)
        {
            await _iRdstationIntegrationService.GetCellphoneNumbersAsync(dealId, token);
        }
    }
}
