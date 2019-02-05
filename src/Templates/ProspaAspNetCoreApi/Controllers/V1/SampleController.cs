using System.Net;
using Microsoft.AspNetCore.Mvc;
using ProspaAspNetCoreApi.Routing;

namespace ProspaAspNetCoreApi.Controllers.V1
{
    [ApiController]
    [V1, VersionedRoute("")]
    public class SampleController : ControllerBase
    {
        [Route("payment-methods")]
        [HttpGet]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public ActionResult<string> Get()
        {
            return Ok("sample");
        }
    }
}
