using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ElasticSearchDotNet.Controllers
{
    [ApiController]
    [Route("api/v1/logs")]
    public class LogsController : ControllerBase
    {
        private readonly ILogger<LogsController> _logger;

        public LogsController(ILogger<LogsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogDebug("Debug do log");
            _logger.LogCritical("Um log critico");
            _logger.LogError("Um log de erro foi lançado");
            _logger.LogInformation("Log de information para exemplo");
            _logger.LogTrace("Log de Trace, funciona?");
            _logger.LogWarning("Ops não é possível avançar sem preencher os parametros de entrada");
            return Ok();
        }

        [HttpGet("error")]
        public IActionResult Error()
        {
            _logger.LogError("Ocorreu um problema ao soliticar essa requisição. Um novo processe será lançada dentro de 5 minutos.");
            return BadRequest();
        }
    }
}
