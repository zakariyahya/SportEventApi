using Newtonsoft.Json;

namespace SportEventsApiServices.Models.Request
{
    public class CreateUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }

    }
}
