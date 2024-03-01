using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Xml;

namespace SportEventsApiServices
{
    public class BaseFunction
    {

        protected IActionResult NotFound(object message)
        {
            return new NotFoundObjectResult(new
            {
                code = 404,
                status = "Not Found",
                message
            });
        }

        protected IActionResult BadRequest(string message)
        {
            return new BadRequestObjectResult(new
            {
                code = 400,
                status = "Bad Request",
                message,
            });
        }

        protected IActionResult NoContent(string message)
        {
            return new NoContentResult();
        }

        protected IActionResult Ok<T>(T value)
        {
            return new JsonResult(new
            {
                Code = 200,
                Message = "Success",
                Data = value,
            });
        }

        protected IActionResult Created<T>(T value)
        {
            return new CreatedResult("", new
            {
                Code = 201,
                Message = "Created",
                Data = value,
            });
        }

        protected IActionResult Accepted()
        {
            return new AcceptedResult();
        }

        protected IActionResult Conflict(string message)
        {
            return new ConflictObjectResult(new
            {
                Code = 409,
                Message = "Conflict",
                Data = message,
            });
        }

        protected IActionResult Unauthorized(string message)
        {
            return new UnauthorizedObjectResult(new
            {
                Code = 401,
                Message = "Unauthorized",
                Data = message,
            });
        }

        protected IActionResult Forbidden(string message)
        {
            return new ForbidResult();
        }

        protected IActionResult InternalServerError(string message)
        {
            return new ObjectResult(new
            {
                Code = 500,
                Message = "Internal Server Error",
                Data = message,
            })
            {
                StatusCode = 500,
            };
        }

    }
}
