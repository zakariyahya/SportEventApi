namespace SportEventsApiServices.Services.Auth
{
    public interface IAuthService
    {
        bool CheckUserAsync (string email);
    }
}
