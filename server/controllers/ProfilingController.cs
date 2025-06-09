using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Profiling;
using StackExchange.Profiling.Internal;

namespace server.Controllers
{
    [Route("api/mini-profiler/")]
    [ApiController]
    public class ProfilingController : ControllerBase
    {

        /*
                [HttpGet("last")]
                public ActionResult GetLastProfilingSession()
                {
                    var _profiler = MiniProfiler.Current;
                    if (_profiler == null)
                    {
                        return NotFound("No profiling session found.");
                    }
                    return Ok(_profiler.ToJson());
                }
                */
    }
}
