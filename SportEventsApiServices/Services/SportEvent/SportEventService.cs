using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SportEventsApiServices.Models;
using SportEventsApiServices.Models.Organizer;
using SportEventsApiServices.Models.SportEvent.Request;
using SportEventsApiServices.Models.SportEvent.Response;

namespace SportEventsApiServices.Services
{
    public class SportEvent : ISportEventService
    {
        private readonly SportEventContextClass _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public SportEvent(SportEventContextClass context, IMapper mapper, IUserService userService) 
        { 
            _context = context;
            _mapper = mapper;
            _userService = userService;
        }
        public async Task<SportEventReadResponse> CreateAsync(CreateSportEvent request)
        {
            var map = _mapper.Map<SportEventModel>(request);

            var existingOrganizer = await ExistingOrganizer(request.OrganizerId).ConfigureAwait(false);

            if (existingOrganizer == null)
            {
                return null;
            }
            map.Organizer = existingOrganizer;

            _context.SportEvents.Add(map);

            await SaveChanges().ConfigureAwait(false);

            return _mapper.Map<SportEventReadResponse>(map);
        }

        public async Task<PaginationResponse<SportEventReadResponse>> GetAsync(int page, int perPage)
        {
            var totalItems = _context.SportEvents.Count();

            var itemsToSkip = (page - 1) * perPage;

            var result = _context.SportEvents
                .Include(x=> x.Organizer)
                .Where(x => x.ActiveFlag == "Y")
                .Skip(itemsToSkip)
                .Take(perPage)
                .ToList();

            var response = _mapper.Map<List<SportEventReadResponse>>(result);

            return new PaginationResponse<SportEventReadResponse>()
            {
                PageSize = page,
                PageNumber = perPage,
                TotalPage = (int)Math.Ceiling((double)totalItems / perPage),
                TotalItem = totalItems,
                Items = response
            };
        }

        public async Task<SportEventReadResponse> GetByIdAsync(int id)
        {
            var data = await GetById(id).ConfigureAwait(false);

            return _mapper.Map<SportEventReadResponse>(data);
        }

        public async Task<SportEventReadResponse> UpdateAsync(int id, UpdateSportEvent request)
        {
            var model = _mapper.Map<SportEventModel>(request);
            var data = await GetById(id).ConfigureAwait(false);

            if(data == null)
            {
                return null;
            }
            var organizer = await ExistingOrganizer(request.OrganizerId).ConfigureAwait(false);
            if (organizer == null)
            {
                return null;
            }
            data.EventDate = model.EventDate;
            data.EventType = model.EventType;
            data.EventName = model.EventName;
            data.LastModifiedBy = _userService.UserId;
            data.LastModifiedTime = DateTime.Now;
            data.Organizer = organizer;

            _context.SportEvents.Update(data);
            await SaveChanges().ConfigureAwait(false);

            return _mapper.Map<SportEventReadResponse>(data);
        }

        public async Task<SportEventReadResponse> DeleteAsync(int id)
        {
            var data = await GetById(id).ConfigureAwait(false);
            if (data == null)
            {
                return null;
            }
            data.LastModifiedBy = _userService.UserId;
            data.LastModifiedTime = DateTime.Now;
            data.ActiveFlag = "N";

            _context.SportEvents.Update(data);
            await SaveChanges().ConfigureAwait(false);

            return _mapper.Map<SportEventReadResponse>(data);
        }

        #region local function
        public async Task<bool> SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        private async Task<SportEventModel> GetById(int id)
        {
            var data = _context.SportEvents.Include(x => x.Organizer).FirstOrDefault(x => x.Id == id && x.ActiveFlag == "Y");
            if (data == null)
            {
                return null;
            }
            return data;

        }

        private async Task<OrganizerModel> ExistingOrganizer(int id)
        {
            var existingOrganizer = _context.Organizers.FirstOrDefault(x => x.ActiveFlag == "Y" && x.Id == id);

            if (existingOrganizer == null)
            {
                return null;
            }
            return existingOrganizer;

        }

        #endregion
    }
}
