using AutoMapper;
using SportEventsApiServices.Models;
using SportEventsApiServices.Models.Request;
using SportEventsApiServices.Models.Response;

namespace SportEventsApiServices.Profiles
{
    public class OrganizerProfile : Profile
    {
        public OrganizerProfile() {
            CreateMap<OrganizerCreateRequest, OrganizerModel>();
            CreateMap<OrganizerUpdateRequest, OrganizerModel>();


            CreateMap<OrganizerModel, OrganizerReadResponse>();
        }
    }
}
