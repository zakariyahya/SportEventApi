using Newtonsoft.Json;

namespace SportEventsApiServices.Models.Response
{
    public class OrganizerReadResponse
    {
        public string Id { get; set; }
        public string OrganizerName { get; set; }
        public string ImageLocation { get; set; }
    }
}
