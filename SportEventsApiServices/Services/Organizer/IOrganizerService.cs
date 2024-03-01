using SportEventsApiServices.Models;
using SportEventsApiServices.Models.Organizer.Request;
using SportEventsApiServices.Models.Organizer.Response;

namespace SportEventsApiServices.Services
{
    public interface IOrganizerService
    {
        Task<OrganizerReadResponse> CreateAsync(OrganizerCreateRequest request);
        Task<PaginationResponse<OrganizerReadResponse>> GetAsync(int page, int perPage);
        Task<OrganizerReadResponse> GetByIdAsync(int id);
        Task<OrganizerReadResponse> UpdateAsync(OrganizerUpdateRequest request, int id);
        Task<OrganizerReadResponse> DeleteAsync(int id);




    }
}
