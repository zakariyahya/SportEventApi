using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SportEventsApiServices.Models;
using SportEventsApiServices.Models.Organizer;
using SportEventsApiServices.Models.Organizer.Request;
using SportEventsApiServices.Models.Organizer.Response;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SportEventsApiServices.Services
{
    public class OrganizerService : IOrganizerService
    {
        private readonly SportEventContextClass _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public OrganizerService(SportEventContextClass context, IMapper mapper, IUserService userService) 
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
        }
        public async Task<OrganizerReadResponse> CreateAsync(OrganizerCreateRequest request)
        {
            var model = _mapper.Map<OrganizerModel>(request);
            _context.Organizers.Add(model);
            await SaveChanges().ConfigureAwait(false);
            return _mapper.Map<OrganizerReadResponse>(model);
        }

        public async Task<OrganizerReadResponse> DeleteAsync(int id)
        {
            var data = await GetById(id).ConfigureAwait(false);

            if (data == null)
            {
                return null;
            }

            data.ActiveFlag = "N";
            data.LastModifiedBy = _userService.UserId;
            data.LastModifiedTime = DateTime.Now;

            _context.Organizers.Update(data);
            await SaveChanges().ConfigureAwait(false);

            return _mapper.Map<OrganizerReadResponse>(data);
        }

        public async Task<PaginationResponse<OrganizerReadResponse>> GetAsync(int page, int perPage)
        {
            var totalItems = _context.Organizers.Count();

            var itemsToSkip = (page - 1) * perPage;

            var result = _context.Organizers
                .Where(x=> x.ActiveFlag == "Y")
                .Skip(itemsToSkip)
                .Take(perPage)
                .ToList();

            var response = _mapper.Map<List<OrganizerReadResponse>>(result);

            return new PaginationResponse<OrganizerReadResponse>()
            {
                PageSize = page,
                PageNumber = perPage,
                TotalPage = (int)Math.Ceiling((double)totalItems / perPage),
                TotalItem = totalItems,
                Items = response
            };
        }

        public async Task<OrganizerReadResponse> GetByIdAsync(int id)
        {
            var data = await GetById(id).ConfigureAwait(false);

            return _mapper.Map<OrganizerReadResponse>(data);
            
        }

        public async Task<OrganizerReadResponse> UpdateAsync(OrganizerUpdateRequest request, int id)
        {
            var map = _mapper.Map<OrganizerModel>(request);

            var data = await ExistingOrganizer(id).ConfigureAwait(false);

            if (data == null)
            {
                return null;
            }

            data.OrganizerName = request.OrganizerName;
            data.ImageLocation = request.ImageLocation;
            data.LastModifiedBy = _userService.UserId;
            data.LastModifiedTime = DateTime.Now;

            _context.Organizers.Update(data);
            SaveChanges();

            map.Id = data.Id;
            return _mapper.Map<OrganizerReadResponse>(map); 
        }
        #region Local Function
        public async Task<bool> SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
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
        private async Task<OrganizerModel> GetById(int id)
        {
            var data = _context.Organizers.FirstOrDefault(x => x.Id == id && x.ActiveFlag == "Y");
            if (data == null)
            {
                return null;
            }
            return data;

        }
        #endregion
    }
}
