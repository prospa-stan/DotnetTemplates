using Microsoft.AspNetCore.Mvc;
using ProspaAspNetCoreApi.Routing;

namespace ProspaAspNetCoreApi.Controllers
{
    [ApiController]
    [V1, VersionedRoute("lines")]
    public class TestController : ControllerBase
    {
        /// <summary>
        /// Redirects to the swagger page.
        /// </summary>
        /// <returns>A 301 Moved Permanently response.</returns>
        [HttpGet]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
