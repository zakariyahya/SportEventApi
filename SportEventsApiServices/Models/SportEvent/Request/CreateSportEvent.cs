using Newtonsoft.Json;

namespace SportEventsApiServices.Models.SportEvent.Request
{
    public class CreateSportEvent
    {
        public DateTime EventDate { get; set; }
        public string EventType { get; set; }
        public string EventName { get; set; }
        public int OrganizerId { get; set; }
    }
}
