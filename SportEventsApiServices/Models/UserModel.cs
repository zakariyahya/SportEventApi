using Newtonsoft.Json;

namespace SportEventsApiServices.Models
{
    public class UserModel : BaseModel
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("passwordHash")]
        public byte[]? PasswordHash { get; set; }

        [JsonProperty("passwordSalt")]
        public byte[]? PasswordSalt { get; set; }

    }
}
