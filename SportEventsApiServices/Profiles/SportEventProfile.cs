using AutoMapper;
using SportEventsApiServices.Models;
using SportEventsApiServices.Models.SportEvent.Request;
using SportEventsApiServices.Models.SportEvent.Response;

namespace SportEventsApiServices.Profiles
{
    public class SportEventProfile : Profile
    {
        public SportEventProfile() {
            CreateMap<CreateSportEvent, SportEventModel>();
            CreateMap<UpdateSportEvent, SportEventModel>();


            CreateMap<SportEventModel, SportEventReadResponse>();
        }
    }
}
