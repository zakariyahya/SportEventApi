namespace SportEventsApiServices.Services.Auth
{
    public interface IAuthService
    {
        Task<bool> CheckUserAsync (string email);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        string CreateToken(string userId, string email);

    }
}
