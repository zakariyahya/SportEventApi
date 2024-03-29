﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportEventsApiServices.Models.Organizer;
using SportEventsApiServices.Models.Organizer.Request;
using SportEventsApiServices.Services;

namespace SportEventsApiServices.Controllers
{
    [Authorize]
    [Route("api/v1")]
    [ApiController]
    public class OrganizerController : BaseFunction
    {
        private readonly IOrganizerService _repository;
        private readonly ILogger<OrganizerModel> _logger;

        public OrganizerController( 
            IOrganizerService organizerService, 
            ILogger<OrganizerModel> logger
            ) 
        {
            _repository = organizerService;
            _logger = logger;
        }

        [Route("organizers")]
        [HttpPost]
        public async Task<IActionResult> CreateOrganizer([FromBody] OrganizerCreateRequest request)
        {
            try
            {
                var response = await _repository.CreateAsync(request).ConfigureAwait(false);
                if(response == null)
                {
                    return BadRequest("Bad Request!");
                }

                return Created(response);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal Server Error");
                return InternalServerError("Internal Server Error");
            }

        }

        [Route("organizers")]
        [HttpGet]
        public async Task<IActionResult> GetOrganizer([FromQuery] int page = 1, int perPage = 10)
        {
            try
            {

                var response = await _repository.GetAsync(page, perPage).ConfigureAwait(false);

                if (!response.Items.Any())
                {
                    return NoContent($"Data is empty");
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal Server Error");
                return InternalServerError("Internal Server Error");
            }

        }

        [Route("organizers/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var response = await _repository.GetByIdAsync(id).ConfigureAwait(false);

                if (response == null)
                {
                    return NotFound("Id not found!");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal Server Error");
                return InternalServerError("Internal server error!");
            }
        }
        [Route("organizers/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] OrganizerUpdateRequest request)
        {
            try
            {
                var response = await _repository.UpdateAsync(request, id).ConfigureAwait(false);

                if (response == null)
                {
                    return NotFound($"Organizer with ID {id} not found");

                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal Server Error");
                return InternalServerError("Internal Server Error");
            }
        }

        [Route("organizers/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteOrganizer(int id)
        {
            try
            {
                var response = await _repository.DeleteAsync(id).ConfigureAwait(false);

                if (response == null)
                {
                    return NotFound($"Organizer with ID {id} not found");

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
