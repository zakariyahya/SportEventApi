using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SportEventsApiServices.Models;
using SportEventsApiServices.Models.Request;
using SportEventsApiServices.Models.Response;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SportEventsApiServices.Services
{
    public class OrganizerService : IOrganizerService
    {
        private readonly SportEventContextClass _context;
        private readonly IMapper _mapper;

        public OrganizerService(SportEventContextClass context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<OrganizerReadResponse> CreateAsync(OrganizerCreateRequest request)
        {
            var model = _mapper.Map<OrganizerModel>(request);
            _context.Organizers.Add(model);
            SaveChanges();
            return _mapper.Map<OrganizerReadResponse>(model);
        }

        public async Task<OrganizerReadResponse> DeleteAsync(int id)
        {
            var data = _context.Organizers.Find(id);

            if (data == null)
            {
                return null;
            }

            data.ActiveFlag = "N";
            data.LastModifiedBy = "System";
            data.LastModifiedTime = DateTime.Now;

            _context.Organizers.Update(data);
            SaveChanges();

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
            var data = _context.Organizers.FirstOrDefault(x => x.ActiveFlag == "Y" && x.Id == id);

            return _mapper.Map<OrganizerReadResponse>(data);
            
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public async Task<OrganizerReadResponse> UpdateAsync(OrganizerUpdateRequest request, int id)
        {
            var map = _mapper.Map<OrganizerModel>(request);

            /*    var data = _context.Organizers.Find(id);*/
            var data = _context.Organizers.FirstOrDefault(x => x.ActiveFlag == "Y" && x.Id == id);

            if (data == null)
            {
                return null;
            }

            data.OrganizerName = request.OrganizerName;
            data.ImageLocation = request.ImageLocation;

            _context.Organizers.Update(data);

            SaveChanges();

            map.Id = data.Id;
            return _mapper.Map<OrganizerReadResponse>(map); 
        }
    }
}
