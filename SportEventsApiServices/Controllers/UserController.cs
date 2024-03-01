using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportEventsApiServices.Models.User;
using SportEventsApiServices.Models.User.Request;
using SportEventsApiServices.Models.User.Response;
using SportEventsApiServices.Services;
using SportEventsApiServices.Services.Auth;
namespace SportEventsApiServices.Controllers
{
    [Authorize]
    [Route("api/v1")]
    [ApiController]
    public class UserController : BaseFunction
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly ILogger<UserModel> _logger;

        public UserController(
            IUserService userService, 
            IAuthService authService, 
            ILogger<UserModel> logger
            )
        {
            _userService = userService;
            _authService = authService;
            _logger = logger;
        }

        [AllowAnonymous]
        [Route("users")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] CreateUser request)
        {
            try
            {

                var check = await _authService.CheckUserAsync(request.Email).ConfigureAwait(false);
                if (!check)
                {
                    return BadRequest("Email already exist!");
                }

                if (request.Password != request.RepeatPassword)
                {
                    return BadRequest("Password and repeat password must be same!");
                }

                var response = await _userService.RegisterAsync(request).ConfigureAwait(false);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal Server Error");

                return InternalServerError("Internal Server Error");
            }

       
        }

        [AllowAnonymous]
        [Route("users/login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel request)
        {
            try
            {

                var data = await _userService.LoginAsync(request).ConfigureAwait(false);

                var token = _authService.CreateToken(data.Id.ToString(), data.Email);
                if (!token.Any())
                {
                    return BadRequest("Invalid");
                }

                return Ok(new LoginResponse
                {
                    Id = data.Id,
                    Email = data.Email,
                    Token = token
                });
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex, "Internal Server Error");

                return InternalServerError("Internal Server Error");
            }
   
        }

        [Route("users/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var response = await _userService.GetByIdAsync(id).ConfigureAwait(false);

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

        [Route("users/{id}")]
        [HttpPut]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUser request)
        {
            try
            {
                var response = await _userService.UpdateAsync(request, id).ConfigureAwait(false);

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

        [Route("users/{id}/password")]
        [HttpPut]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangePassword request)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id).ConfigureAwait(false);

                if (user == null)
                {
                    return NotFound($"User with ID {id} not found");
                }
                if(request.NewPassword != request.RepeatPassword)
                {
                    return BadRequest("Password and repeat password must be same!");
                }

                if (!_userService.CheckOldPassword(request.OldPassword, id).Result)
                {
                    return BadRequest("Password is Incorrect!");
                }
                var response = await _userService.ChangePassword(request, id).ConfigureAwait(false);

                if (response != null)
                {
                    return Ok(response);
                }
                else
                {
                    return NotFound($"User with ID {id} not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal Server Error");

                return InternalServerError("Internal Server Error");
            }
        }

        [Route("users/{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _userService.DeleteAsync(id).ConfigureAwait(false);

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

    }
}
