using Microsoft.AspNetCore.Mvc;
using MoveEnergia.Billing.Core.Dto;
using MoveEnergia.Billing.Core.Dto.Request;
using MoveEnergia.RdStation.Adapter.Interface.Adapter;
using MoveEnergia.RdStation.Adapter.Interface.Service;
using Serilog;
using System.Text;

namespace MoveEnergia.Billing.Api.Controllers
{
    [ApiController]
    [Route("api/RdStation")]
    public class RdStationController : ControllerBase
    {
        private readonly ILogger<RdStationController> _logger;
        private readonly IRdstationIntegrationAdapter _iRdstationIntegrationAdapter;
        private readonly IRdCargaService _iRdCargaService;

        public RdStationController(ILogger<RdStationController> logger, IRdstationIntegrationAdapter iRdstationIntegrationAdapter, IRdCargaService iRdCargaService)
        {
            _logger = logger;
            _iRdstationIntegrationAdapter = iRdstationIntegrationAdapter;
            _iRdCargaService = iRdCargaService;
        }

        /*[HttpGet]
        [Route("Contacts/{dealId}")]
        [ProducesResponseType(typeof(ReturnResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetContactsAsync(string dealId)
        {
            var retornoDto = await _iRdstationIntegrationAdapter.GetContactsAsync(dealId);
            return StatusCode(retornoDto.StatusCode, retornoDto);
        }

        [HttpPost]
        [Route("Fetch/Unity")]
        [ProducesResponseType(typeof(ReturnResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FetchUnidadesPageAsync([FromBody] FetchUnidadesPageRequestDto request)
        {
             var retornoDto = await _iRdstationIntegrationAdapter.FetchUnidadesPageAsync(request.page, request.limit, request.next_page);
            return StatusCode(retornoDto.StatusCode, retornoDto);
        }

        [HttpPost]
        [Route("Fetch/Unity/{dealId}")]
        [ProducesResponseType(typeof(ReturnResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FetchUnidadesFromRdStationAsync(string dealId, [FromBody] FetchUnidadesFromRdStationRequestDto request)
        {
            var retornoDto = await _iRdstationIntegrationAdapter.FetchUnidadesFromRdStationAsync(dealId, request.isStage, request.page, request.limit);
            return StatusCode(retornoDto.StatusCode, retornoDto);
        }

        [HttpPost]
        [Route("Sync/Customer")]
        [ProducesResponseType(typeof(ReturnResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SyncCustomerAsync([FromBody] SyncCustomerRequestDto request)
        {
            var retornoDto = await _iRdstationIntegrationAdapter.SyncCustomerAsync(request);
            return StatusCode(retornoDto.StatusCode, retornoDto);
        }

        [HttpPost]
        [Route("Sync/Customer/STG/UCs")]
        [ProducesResponseType(typeof(ReturnResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SyncCustomerUCAsync([FromBody] string listUCs)
        {
            var retornoDto = await _iRdstationIntegrationAdapter.SyncCustomerListUCAsync(listUCs);
            return StatusCode(retornoDto.StatusCode, retornoDto);
        }*/

        [HttpGet]
        [Route("Carga/ListPipelines")]
        [ProducesResponseType(typeof(ReturnResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListPipelines()
        {
            Log.Debug("teste");
            var retornoDto = await _iRdCargaService.GetPipelines();
            return StatusCode(retornoDto.StatusCode, retornoDto);
        }

        [HttpGet]
        [Route("Carga/Address")]
        [ProducesResponseType(typeof(ReturnResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CargaEnderecoAsync()
        {
            Log.Debug("teste");
            var retornoDto = await _iRdCargaService.CargaEnderecosAsync(0, 100, "");
            return StatusCode(retornoDto.StatusCode, retornoDto);
        }
    }
}
