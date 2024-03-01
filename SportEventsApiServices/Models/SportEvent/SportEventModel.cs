using Newtonsoft.Json;
using SportEventsApiServices.Models.Organizer;

namespace SportEventsApiServices.Models
{
    public class SportEventModel : BaseModel
    {
        [JsonProperty("eventDate")]
        public DateTime EventDate { get; set; }
        [JsonProperty("eventType")]
        public string EventType { get; set; }
        [JsonProperty("eventName")]
        public string EventName { get; set; }
        [JsonProperty("organizer")]
        public OrganizerModel Organizer {  get; set; }
    }
}
