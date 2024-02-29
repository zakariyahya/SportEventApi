using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportEventsApiServices.Models.Request;
using SportEventsApiServices.Services;

namespace SportEventsApiServices.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class OrganizerController : BaseFunction
    {
        private readonly IOrganizerService _repository;
        public OrganizerController( IOrganizerService organizerService) 
        {
            _repository = organizerService;
        }

        [Route("organizers")]
        [HttpPost]
        public IActionResult CreateOrganizer([FromBody] OrganizerCreateRequest request)
        {

            var response = _repository.CreateAsync(request);

            return Ok(response.Result);
        }

        [Route("organizers/all")]
        [HttpGet]
        public IActionResult GetOrganizer([FromQuery] int page = 1, int perPage = 10)
        {
            try
            {

                var response = _repository.GetAsync(page, perPage);

                if (response.Result.Items.Any())
                {
                    return Ok(response.Result);
                }
                else
                {
                    return NoContent($"Organizer data is empty");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Internal Server Error");
            }

        }

        [Route("organizers/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetOrganizerById(int id)
        {
            try
            {
                var response = await _repository.GetByIdAsync(id);

                if (response != null)
                {
                    return Ok(response);
                }
                else
                {
                    return NotFound($"Organizer with ID {id} not found");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return BadRequest("Internal Server Error");
            }
        }
        [Route("organizers/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateOrganizer(int id, [FromBody] OrganizerUpdateRequest request)
        {
            try
            {
                var response = await _repository.UpdateAsync(request, id);

                if (response != null)
                {
                    return Ok(response);
                }
                else
                {
                    return NotFound($"Organizer with ID {id} not found");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return BadRequest("Internal Server Error");
            }
        }

        [Route("organizers/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteOrganizer(int id)
        {
            try
            {
                var response = await _repository.DeleteAsync(id);

                if (response != null)
                {
                    return Ok(response);
                }
                else
                {
                    return NotFound($"Organizer with ID {id} not found");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return BadRequest("Internal Server Error");
            }
        }

    }
}
