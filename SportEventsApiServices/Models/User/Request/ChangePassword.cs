namespace SportEventsApiServices.Models.User.Request
{
    public class ChangePassword
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string RepeatPassword { get; set; }
    }
}
