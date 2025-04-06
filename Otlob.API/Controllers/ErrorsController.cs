using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Otlob.API.Errors;

namespace Otlob.API.Controllers
{
    [Route("errors/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        public ActionResult Error(int code)
        {
            return NotFound(new ErrorResponse(code));
        }
    }
}
