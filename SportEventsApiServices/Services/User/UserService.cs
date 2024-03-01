using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using SportEventsApiServices.Models;
using SportEventsApiServices.Models.User;
using SportEventsApiServices.Models.User.Request;
using SportEventsApiServices.Models.User.Response;
using SportEventsApiServices.Services.Auth;
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
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(SportEventContextClass context, IMapper mapper, IConfiguration configuration, IAuthService authService, IHttpContextAccessor httpContextAccessor) 
        { 
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId => GetUserId();

        private string GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext.User.Identity.Name;
            if (!string.IsNullOrEmpty(userId))
                return userId;
            return GetUserFromHeader(_httpContextAccessor.HttpContext.Request);
        }

        public async Task<UserReadResponse> RegisterAsync(CreateUser request)
        {
            var model = _mapper.Map<UserModel>(request);

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            model.PasswordHash = passwordHash;
            model.PasswordSalt = passwordSalt;

            _context.Add(model);
            await SaveChanges().ConfigureAwait(false);

            return _mapper.Map<UserReadResponse>(model);
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

        public async Task<UserReadResponse> GetByIdAsync(int id)
        {
            var data = await GetById(id).ConfigureAwait(false);

            return _mapper.Map<UserReadResponse>(data);
        }

        public async Task<UserReadResponse> UpdateAsync(UpdateUser request, int id)
        {
            var map = _mapper.Map<UserModel>(request);

            var data = await GetById(id).ConfigureAwait(false);

            data.FirstName = request.FirstName;
            data.LastName = request.LastName;
            data.Email = request.Email;
            data.LastModifiedBy = UserId;
            data.LastModifiedTime = DateTime.Now;

            _context.Users.Update(data);

            await SaveChanges().ConfigureAwait(false);

            map.Id = data.Id;
            return _mapper.Map<UserReadResponse>(map);
        }

        public async Task<UserReadResponse> ChangePassword(ChangePassword request, int id)
        {
            var data = await GetById(id).ConfigureAwait(false);

            CreatePasswordHash(request.OldPassword, out byte[] passwordHash, out byte[] passwordSalt);

            data.PasswordHash = passwordHash;
            data.PasswordSalt = passwordSalt;

            data.LastModifiedBy = "System";
            data.LastModifiedTime = DateTime.Now;

            _context.Users.Update(data);
            await SaveChanges().ConfigureAwait(false);

            return _mapper.Map<UserReadResponse>(data);
        }

        public async Task<UserReadResponse> DeleteAsync(int id)
        {
            var data = await GetById(id).ConfigureAwait(false);

            if (data == null)
            {
                return null;
            }

            data.ActiveFlag = "N";
            data.LastModifiedBy = UserId;
            data.LastModifiedTime = DateTime.Now;

            _context.Users.Update(data);
            await SaveChanges().ConfigureAwait(false);

            return _mapper.Map<UserReadResponse>(data);
        }

        public async Task<bool> CheckOldPassword(string oldPassword, int id)
        {
            var data = await GetById(id).ConfigureAwait(false);

            if (!VerifyPasswordHash(oldPassword, data.PasswordHash, data.PasswordSalt))
            {
                return false;
            }
            return true;
        }

        #region Local Function
        public async Task<bool> SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
        private async Task<UserModel> GetById(int id)
        {
            var data = _context.Users.FirstOrDefault(x => x.Id == id && x.ActiveFlag == "Y");
            if (data == null)
            {
                return null;
            }
            return data;

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

        private static string GetUserFromHeader(HttpRequest req)
        {
            bool authorize = req.Headers.TryGetValue("Authorization", out var val);
            if (authorize)
            {
                var token = ((string)val).Replace("Bearer ", string.Empty);
                var jwt = new JwtSecurityToken(token);
                var user = jwt?.Claims.FirstOrDefault(c => c.Type == "upn");
                if (user != null)
                {
                    return user.Value;
                }
                return jwt?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            }
            return null;
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

        #endregion
    }
}
