namespace SportEventsApiServices.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly SportEventContextClass _context;

        public AuthService(SportEventContextClass context) { 
            _context = context;
        }
        public bool CheckUserAsync(string email)
        {
           var user = _context.Users.FirstOrDefault(x => x.Email == email);
            if(user == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
