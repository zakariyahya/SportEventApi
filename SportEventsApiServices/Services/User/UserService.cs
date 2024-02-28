using AutoMapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.IdentityModel.Tokens;
using SportEventsApiServices.Models;
using SportEventsApiServices.Models.Request;
using SportEventsApiServices.Models.Response;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SportEventsApiServices.Services
{
    public class UserService : IUserService
    {
        private readonly SportEventContextClass _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserService(SportEventContextClass context, IMapper mapper, IConfiguration configuration) 
        { 
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task<UserReadResponse> CreateAsync(CreateUser request)
        {
            var model = _mapper.Map<UserModel>(request);

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            model.PasswordHash = passwordHash;
            model.PasswordSalt = passwordSalt;

            _context.Add(model);
            SaveChanges();

            return _mapper.Map<UserReadResponse>(model);
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

        public async Task<LoginResponse> LoginAsync(LoginModel request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return null;
            }
            var user = _context.Users.SingleOrDefault(x => x.Email == request.Email);
            if (user == null)
            {
                return null;
            }
            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }
         
            return _mapper.Map<LoginResponse>(user);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        private static bool VerifyPasswordHash(
            string password,
            byte[] storedHash,
            byte[] storedSalt
        )
        {
            if (password == null)
                throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException(
                    "Value cannot be empty or whitespace only string.",
                    "password"
                );
            if (storedHash.Length != 64)
                throw new ArgumentException(
                    "Invalid length of password hash (64 bytes expected).",
                    "passwordHash"
                );
            if (storedSalt.Length != 128)
                throw new ArgumentException(
                    "Invalid length of password salt (128 bytes expected).",
                    "passwordHash"
                );
            using var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != storedHash[i])
                    return false;
            }
            return true;
        }

        /*      public string CreateToken(UserModel request)
              {
                  var tokenHandler = new JwtSecurityTokenHandler();
                  var key = Encoding.ASCII.GetBytes("MY-SECRETTTT");
                  var tokenDescriptor = new SecurityTokenDescriptor
                  {
                      Subject = new ClaimsIdentity(new Claim[]
                      {
                      new Claim(ClaimTypes.Email, request.Email)
                      }),
                      Expires = DateTime.UtcNow.AddDays(7),
                      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                  };

                  var token = tokenHandler.CreateToken(tokenDescriptor);
                  var tokenString = tokenHandler.WriteToken(token);

                  return tokenString;
              }*/
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
    }
}
