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
            return new BadRequestObjectResult(new
            {
                code = 204,
                status = "No Content",
                message,
            });
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
    }
}
