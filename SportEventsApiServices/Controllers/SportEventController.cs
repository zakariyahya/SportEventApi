using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportEventsApiServices.Services.Auth;
using SportEventsApiServices.Services;
using Microsoft.AspNetCore.Authorization;
using SportEventsApiServices.Models.SportEvent.Request;
using SportEventsApiServices.Models;
namespace SportEventsApiServices.Controllers
{
    [Authorize]
    [Route("api/v1")]
    [ApiController]
    public class SportEventController : BaseFunction
    {
        private readonly ISportEventService _repository;
        private readonly ILogger<SportEventModel> _logger;

        public SportEventController(
            ISportEventService repository, 
            ILogger<SportEventModel> logger
            )
        {
            _repository = repository;
            _logger = logger;
        }

        [Route("sport-events")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateSportEvent request)
        {
            try
            {
                var response = await _repository.CreateAsync(request).ConfigureAwait(false);
                if(response == null)
                {
                    return NotFound("OrganizerId not found");
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal Server Error");

                return InternalServerError("Internal Server Error");
            }
        }

        [Route("sport-events")]
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] int page = 1, int perPage = 10)
        {
            try
            {
                var response = await _repository.GetAsync(page, perPage).ConfigureAwait(false);
                if (!response.Items.Any())
                {
                    return BadRequest("Data is empty");
                }
                return Ok(response);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal Server Error");

                return InternalServerError("Internal Server Error");
            }
        }

        [Route("sport-events/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var response = await _repository.GetByIdAsync(id).ConfigureAwait(false);
                if (response == null)
                {
                    return NotFound($"Id {id} not found");
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal Server Error");

                return InternalServerError("Internal Server Error");
            }
        }

        [Route("sport-events/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateSportEvent request)
        {
            try
            {
                var response = await _repository.UpdateAsync(id, request).ConfigureAwait(false);
                if (response == null)
                {
                    return NotFound($"Id {id} not found");
                }
                return Ok(response);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal Server Error");

                return InternalServerError("Internal Server Error");
            }
        }

        [Route("sport-events/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var response = await _repository.DeleteAsync(id).ConfigureAwait(false);
                if (response == null)
                {
                    return NotFound($"Id or not found");
                }
                return Ok(response);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal Server Error");

                return InternalServerError("Internal Server Error");

            }
        }
    }
}
