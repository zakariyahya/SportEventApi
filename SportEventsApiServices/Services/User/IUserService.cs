using Microsoft.AspNetCore.Identity.Data;
using SportEventsApiServices.Models;
using SportEventsApiServices.Models.Request;
using SportEventsApiServices.Models.Response;

namespace SportEventsApiServices.Services
{
    public interface IUserService
    {
        Task<UserReadResponse> CreateAsync(CreateUser request);
        Task<LoginResponse> LoginAsync(LoginModel request);

        string CreateToken(string userId, string email);

    }
}
