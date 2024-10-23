using AccountManagment.Core.Models;
using AccountManagment.Core.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http; 
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AccountManagment.Service.Services
{
    public class LoginService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginService(IGenericRepository<User> userRepository, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public async void Logout()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync();

        }
        public async Task<bool> Login(string userName, string password)
        {
            var user = await _userRepository.Where(u => u.Password == password &&
                (u.Email == userName || u.TcNo == userName || u.Phone == userName) && !u.IsDeleted)
                .AsNoTracking().FirstOrDefaultAsync();

            if (user == null)
            {
                return false; 
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Surname, user.Surname),
                new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
            };

            var claimsIdentity = new ClaimsIdentity(claims, "login");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, 
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(15) 
            };

            await _httpContextAccessor.HttpContext.SignInAsync(claimsPrincipal, authProperties);

            return true; 
        }
    }
}
