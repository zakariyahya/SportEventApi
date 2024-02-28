namespace SportEventsApiServices.Models.Request
{
    public class UpdateSportEvent
    {
        public DateTime EventDate { get; set; }
        public string EventType { get; set; }
        public DateTime EventName { get; set; }
        public int OrganizerId { get; set; }
    }
}
