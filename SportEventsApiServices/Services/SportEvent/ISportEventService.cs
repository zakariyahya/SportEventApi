using SportEventsApiServices.Models;
using SportEventsApiServices.Models.Organizer;
using SportEventsApiServices.Models.Organizer.Response;
using SportEventsApiServices.Models.SportEvent.Request;
using SportEventsApiServices.Models.SportEvent.Response;

namespace SportEventsApiServices.Services
{
    public interface ISportEventService
    {
        Task<SportEventReadResponse> CreateAsync(CreateSportEvent request);
        Task<PaginationResponse<SportEventReadResponse>> GetAsync(int page, int perPage);
        Task<SportEventReadResponse> GetByIdAsync(int id);
        Task<SportEventReadResponse> UpdateAsync(int id, UpdateSportEvent request);
        Task<SportEventReadResponse> DeleteAsync(int id);




    }
}
