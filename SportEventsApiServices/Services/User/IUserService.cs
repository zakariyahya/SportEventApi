using Microsoft.AspNetCore.Identity.Data;
using SportEventsApiServices.Models;
using SportEventsApiServices.Models.User;
using SportEventsApiServices.Models.User.Request;
using SportEventsApiServices.Models.User.Response;

namespace SportEventsApiServices.Services
{
    public interface IUserService
    {
        Task<UserReadResponse> RegisterAsync(CreateUser request);
        Task<LoginResponse> LoginAsync(LoginModel request);
        Task<UserReadResponse> GetByIdAsync(int id);
        Task<UserReadResponse> UpdateAsync(UpdateUser request, int id);
        Task<UserReadResponse> ChangePassword(ChangePassword request, int id);
        Task<UserReadResponse> DeleteAsync(int id);
        Task<bool> CheckOldPassword(string oldPassword, int id);
        string CreateToken(string userId, string email);
        string UserId { get; }

    }
}
