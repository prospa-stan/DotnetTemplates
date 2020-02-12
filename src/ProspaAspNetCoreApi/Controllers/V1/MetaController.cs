using Microsoft.AspNetCore.Mvc;

namespace ProspaAspNetCoreApi.Controllers.V1
{
    [Route("")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    [ApiVersionNeutral]
    public class MetaController : ControllerBase
    {
        [HttpGet("api/ping")]
        public ActionResult ApiPing()
        {
            return Ok("pong");
        }

        [HttpGet("ping")]
        public ActionResult<string> Ping()
        {
            return Ok("pong");
        }

        [HttpGet("index")]
        public ActionResult Index()
        {
            return Ok();
        }
    }
}
