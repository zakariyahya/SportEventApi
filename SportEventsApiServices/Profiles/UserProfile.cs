using AutoMapper;
using SportEventsApiServices.Models.User;
using SportEventsApiServices.Models.User.Request;
using SportEventsApiServices.Models.User.Response;
namespace SportEventsApiServices.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile() {
            CreateMap<CreateUser, UserModel>();
            CreateMap<LoginModel, UserModel>();
            CreateMap<UpdateUser, UserModel>();
            CreateMap<ChangePassword, UserModel>();

            CreateMap<UserModel, UserReadResponse>();
            CreateMap<UserModel, LoginResponse>();

        }
    }
}
