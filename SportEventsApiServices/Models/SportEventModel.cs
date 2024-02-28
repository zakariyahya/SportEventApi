using Newtonsoft.Json;

namespace SportEventsApiServices.Models
{
    public class SportEventModel : BaseModel
    {
        [JsonProperty("eventDate")]
        public DateTime EventDate { get; set; }
        [JsonProperty("eventType")]
        public string EventType { get; set; }
        [JsonProperty("eventName")]
        public DateTime EventName { get; set; }
        [JsonProperty("organizer")]
        public OrganizerModel Organizer {  get; set; }
    }
}
