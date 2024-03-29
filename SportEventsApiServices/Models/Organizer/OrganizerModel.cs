﻿using Newtonsoft.Json;

namespace SportEventsApiServices.Models.Organizer
{
    public class OrganizerModel : BaseModel
    {
        [JsonProperty("organizerName")]
        public string OrganizerName { get; set; }
        [JsonProperty("imageLocation")]
        public string ImageLocation { get; set; }
    }
}
