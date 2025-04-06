using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Otlob.API.Errors;
using Otlob.Repository.Data;

namespace Otlob.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : ControllerBase
    {
        private readonly API1DbContext _dbContext;

        public BuggyController(API1DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //[HttpGet("BadRequest")]
        //public ActionResult GetBadRequestError()
        //{
        //    return BadRequest();
        //}

        //[HttpGet("NotFound")]
        //public ActionResult GetNotFoundError()
        //{
        //    var product = _dbContext.Products.Find(100); // Null

        //    return product is null ? NotFound() : Ok(product);
        //}

        [HttpGet("ServerError")]
        public ActionResult GetServerError()
        {
            var product = _dbContext.Products.Find(100); // Null

            var productToReturn = product.ToString(); // NullReferenceException

            return Ok(productToReturn);
        }

        [HttpGet("BadRequest/{id}")]
        public ActionResult GetValidationError(int id)
        {
            return Ok();
        }
        //[HttpGet("Throw")]
        //public IActionResult Throw() =>
        //throw new Exception("Sample exception.");

        [HttpGet("BadRequest")]
        public ActionResult GetBadRequestError()
        {
            return BadRequest(new ErrorResponse(400));
        }


        [HttpGet("NotFound")]
        public ActionResult GetNotFoundError()
        {
            var product = _dbContext.Products.Find(100); // Null

            return product is null ? NotFound(new ErrorResponse(404)) : Ok(product);
        }
    }
}
