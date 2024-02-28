using Newtonsoft.Json;

namespace SportEventsApiServices.Models.Request
{
    public class CreateSportEvent
    {
        public DateTime EventDate { get; set; }
        public string EventType { get; set; }
        public DateTime EventName { get; set; }
        public int OrganizerId { get; set; }
    }
}
