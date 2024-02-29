using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SportEventsApiServices.Models;
using SportEventsApiServices.Models.Request;
using SportEventsApiServices.Models.Response;
using SportEventsApiServices.Services;
using SportEventsApiServices.Services.Auth;
namespace SportEventsApiServices.Controllers
{
/*    [Route("api/[controller]")]*/
    [Route("api/v1")]
    [ApiController]
    public class UserController : BaseFunction
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UserController(IUserService userService, IMapper mapper, IAuthService authService)
        {
            _userService = userService;
            _mapper = mapper;
            _authService = authService;
        }

        [AllowAnonymous]
        [Route("users")]
        [HttpPost]
        public IActionResult Register([FromBody]CreateUser request)
        {
            var check = _authService.CheckUserAsync(request.Email);
            if (!check)
            {
                return BadRequest("Email already exist!");
            }

            if (request.Password != request.RepeatPassword) 
            {
                return BadRequest("Password and repeat password must be same!");
            }

            var response = _userService.CreateAsync(request);

            return Ok(response);
        }

        [AllowAnonymous]
        [Route("users/login")]
        [HttpPost]
        public IActionResult Login([FromBody] LoginModel request)
        {
            var data = _userService.LoginAsync(request);

            var token = _userService.CreateToken(data.Result.Id.ToString(), data.Result.Email);

            return Ok(new LoginResponse
            {
                Id = data.Result.Id,
                Email = data.Result.Email,
                Token = token
            });
        }

    }
}
