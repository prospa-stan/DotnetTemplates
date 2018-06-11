using CorrelationId;
using Microsoft.AspNetCore.Mvc;
using ProspaAspNetCoreApi.Routing;

namespace ProspaAspNetCoreApi.Controllers.V1
{
    [VersionedRoute("[controller]"), V1]
    [ApiController]
    public class CorrelationController : ControllerBase
    {
        private readonly ICorrelationContextAccessor _correlationContext;

        public CorrelationController(ICorrelationContextAccessor correlationContext) { _correlationContext = correlationContext; }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return _correlationContext.CorrelationContext.CorrelationId;
        }
    }
}