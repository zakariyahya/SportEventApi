using AutoMapper;
using SportEventsApiServices.Models;
using SportEventsApiServices.Models.Request;
using SportEventsApiServices.Models.Response;
namespace SportEventsApiServices.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile() {
            CreateMap<CreateUser, UserModel>();
            CreateMap<LoginModel, UserModel>();

            CreateMap<UserModel, UserReadResponse>();
            CreateMap<UserModel, LoginResponse>();

        }
    }
}
