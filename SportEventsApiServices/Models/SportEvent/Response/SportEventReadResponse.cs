using SportEventsApiServices.Models.Organizer.Response;

namespace SportEventsApiServices.Models.SportEvent.Response
{
    public class SportEventReadResponse
    {
        public int Id { get; set; }
        public string EventDate { get; set; }
        public string EventType { get; set; }
        public string eventName { get; set; }
        public OrganizerReadResponse Organizer { get; set; }
    }
}
