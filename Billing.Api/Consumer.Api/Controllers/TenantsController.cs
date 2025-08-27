using Microsoft.AspNetCore.Mvc;
using MoveEnergia.Billing.Core.Dto;
using MoveEnergia.Billing.Core.Interface.Service;
using System.Text;

namespace MoveEnergia.Billing.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantsController : ControllerBase
    {     
        private readonly ILogger<TenantsController> _logger;
        private readonly ITenantsService _tenantsService;

        public TenantsController(ILogger<TenantsController> logger,
                                 ITenantsService tenantsService)
        {            
            _logger = logger;
            _tenantsService = tenantsService;
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
        [Route("{id}")]
        [ProducesResponseType(typeof(ReturnResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var retornoDto = await _tenantsService.GetByIdAsync(id);
            return Ok(retornoDto);
        }
    }
}
