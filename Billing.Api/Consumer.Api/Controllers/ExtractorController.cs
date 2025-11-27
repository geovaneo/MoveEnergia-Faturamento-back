using Microsoft.AspNetCore.Mvc;
using MoveEnergia.Billing.Core.Dto;
using MoveEnergia.Billing.Core.Dto.Request;
using MoveEnergia.Billing.Core.Interface.Service;
using MoveEnergia.RdStation.Adapter.Interface.Adapter;
using MoveEnergia.RdStation.Adapter.Interface.Service;
using Serilog;
using System.Text;

namespace MoveEnergia.Billing.Api.Controllers
{
    [ApiController]
    [Route("api/Extractor")]
    public class ExtractorController : ControllerBase
    {
        private readonly ILogger<RdStationController> _logger;
        private readonly IPdfExtractorService _iPdfExtractorService;

        public ExtractorController(ILogger<RdStationController> logger, IPdfExtractorService iPdfExtractorService)
        {
            _logger = logger;
            _iPdfExtractorService = iPdfExtractorService;
        }

        [HttpGet]
        [Route("Read")]
        [ProducesResponseType(typeof(ReturnResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReadAsync()
        {
            Log.Debug("teste");
            var retornoDto = await _iPdfExtractorService.StartProcess(1);
            return StatusCode(200, retornoDto);
        }
    }
}
