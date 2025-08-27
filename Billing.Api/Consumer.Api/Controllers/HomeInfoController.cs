using Microsoft.AspNetCore.Mvc;
using MoveEnergia.Billing.Core.Dto;
using MoveEnergia.Billing.Core.Interface.Adapter;
using System.Text;

namespace MoveEnergia.Billing.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeInfoController : ControllerBase
    {
        private readonly ILogger<HomeInfoController> _logger;
        private readonly IHomeInfoAdapter _homeInfoAdapter;

        public HomeInfoController(ILogger<HomeInfoController> logger,
                                  IHomeInfoAdapter homeInfoAdapter)
        {
            _logger = logger;
            _homeInfoAdapter = homeInfoAdapter;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(IgnoreApi = true)]

        public IActionResult HealthCheck()
        {
            StringBuilder informacoes = new StringBuilder();
            informacoes.AppendLine($"API MoveEnergia Consumer = MoveEnergia.Billing.Api");
            informacoes.AppendLine($"Situação = Saudável");

            return Ok(informacoes.ToString());
        }

        [HttpGet]
        [Route("Info/UC/{Uc}")]
        [ProducesResponseType(typeof(ReturnResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHomeInfoByIdUCAsync(string Uc)
        {
            var retornoDto = await _homeInfoAdapter.GetHomeInfoByUCAsync(Uc);
            return StatusCode(retornoDto.StatusCode, retornoDto);
        }


        [HttpGet]
        [Route("Info/Id/{idUc}")]
        [ProducesResponseType(typeof(ReturnResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHomeInfoByUCAsync(int idUc)
        {
            var retornoDto = await _homeInfoAdapter.GetHomeInfoByIdUCAsync(idUc);
            return StatusCode(retornoDto.StatusCode, retornoDto);
        }
        [HttpGet]
        [Route("LabelGraphic/{idUc}")]
        [ProducesResponseType(typeof(ReturnResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLabelGraphicByUCAsync(int idUc)
        {
            var retornoDto = await _homeInfoAdapter.GetLabelGraphicByIdUCAsync(idUc);
            return StatusCode(retornoDto.StatusCode, retornoDto);
        }

        [HttpGet]
        [Route("LabelGraphicDetail/{idUc}/{month}/{year}")]
        [ProducesResponseType(typeof(ReturnResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLabelGraphicDetailAsync(int idUc, int month, int year)
        {
            var retornoDto = await _homeInfoAdapter.GetLabelGraphicDetailByIdUCAsync(idUc, month, year);
            return StatusCode(retornoDto.StatusCode, retornoDto);
        }
    }
}
