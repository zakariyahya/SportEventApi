using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SportEventsApiServices.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly SportEventContextClass _context;
        private readonly IConfiguration _configuration;

        public AuthService(SportEventContextClass context, IConfiguration configuration) { 
            _context = context;
            _configuration = configuration;
        }
        public async Task<bool> CheckUserAsync(string email)
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

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            if (password == null)
                throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException(
                    "Value cannot be empty or whitespace only string.",
                    "password"
                );
            if (passwordHash.Length != 64)
                throw new ArgumentException(
                    "Invalid length of password hash (64 bytes expected).",
                    "passwordHash"
                );
            if (passwordSalt.Length != 128)
                throw new ArgumentException(
                    "Invalid length of password salt (128 bytes expected).",
                    "passwordHash"
                );
            using var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != passwordSalt[i])
                    return false;
            }
            return true;
        }

        public string CreateToken(string userId, string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["AppSettings:Token"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                      new Claim(ClaimTypes.Name, userId),

                      new Claim(ClaimTypes.Email,email)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null)
                throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException(
                    "Value cannot be empty or whitespace only string.",
                    "password"
                );

            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}
