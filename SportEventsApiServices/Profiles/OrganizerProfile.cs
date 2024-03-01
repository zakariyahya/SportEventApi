using AutoMapper;
using SportEventsApiServices.Models.Organizer;
using SportEventsApiServices.Models.Organizer.Request;
using SportEventsApiServices.Models.Organizer.Response;

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
