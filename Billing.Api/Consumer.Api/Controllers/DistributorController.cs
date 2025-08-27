using Microsoft.AspNetCore.Mvc;
using MoveEnergia.Billing.Adapter;
using MoveEnergia.Billing.Core.Dto;
using MoveEnergia.Billing.Core.Dto.Request;
using MoveEnergia.Billing.Core.Interface.Adapter;
using System.Text;

namespace MoveEnergia.Billing.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DistributorController : ControllerBase
    {
        private readonly ILogger<DistributorController> _logger;
        private readonly IDistributorAdapter _distributorAdapter;

        public DistributorController(ILogger<DistributorController> logger, IDistributorAdapter distributorAdapter)
        {
            _logger = logger;
            _distributorAdapter = distributorAdapter;
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

        [HttpPost]
        [Route("Save")]
        [ProducesResponseType(typeof(ReturnResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DistributorSave([FromBody] DistributorRequestDto distributor)
        {
            var retornoDto = await _distributorAdapter.CreateAsync(distributor);
            return StatusCode(retornoDto.StatusCode, retornoDto);
        }

        [HttpPut]
        [Route("UpDate")]
        [ProducesResponseType(typeof(ReturnResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DistributorUpDate([FromBody] DistributorRequestDto distributor)
        {
            var retornoDto = await _distributorAdapter.UpDateAsync(distributor);
            return StatusCode(retornoDto.StatusCode, retornoDto);
        }


        [HttpDelete]
        [Route("Delete/{id}")]
        [ProducesResponseType(typeof(ReturnResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DistributorDelete(int id)
        {
            var retornoDto = await _distributorAdapter.DeleteAsync(id);
            return StatusCode(retornoDto.StatusCode, retornoDto);
        }

        [HttpGet]
        [Route("GetAll")]
        [ProducesResponseType(typeof(ReturnResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DistributorGetAll()
        {
            var retornoDto = await _distributorAdapter.GetAllAsync();
            return StatusCode(retornoDto.StatusCode, retornoDto);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ReturnResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DistributorGetById(int id)
        {
            var retornoDto = await _distributorAdapter.GetByIdAsync(id);
            return StatusCode(retornoDto.StatusCode, retornoDto);
        }
    }
}
