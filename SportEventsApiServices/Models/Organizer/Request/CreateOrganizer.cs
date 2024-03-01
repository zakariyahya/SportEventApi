using Newtonsoft.Json;

namespace SportEventsApiServices.Models.Organizer.Request
{
    public class CreateOrganizer
    {
        public string OrganizerName { get; set; }
        public string ImageLocation { get; set; }
    }
}
