using Newtonsoft.Json;

namespace SportEventsApiServices.Models.Organizer.Response
{
    public class OrganizerReadResponse
    {
        public int Id { get; set; }
        public string OrganizerName { get; set; }
        public string ImageLocation { get; set; }
    }
}
