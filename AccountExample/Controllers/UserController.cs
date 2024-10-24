using AccountExample.Models.Accounts;
using AccountExample.Models.Users;
using AccountManagment.Core.Models;
using AccountManagment.Core.Repositories;
using AccountManagment.Core.Services;
using AccountManagment.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AccountExample.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IGenericService<User> _userService;
        private readonly IGenericService<Account> _accountService;
        private readonly LoginService _loginService;

        public UserController(IGenericService<User> userService, IGenericService<Account> accountService, LoginService loginService)
        {
            _userService = userService;
            _accountService = accountService;
            _loginService=loginService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var parsedUserId))
            {
                return RedirectToAction("Login", "User");
            }
            var user=await _userService.GetByIdAsync(parsedUserId);
            return View(user);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            LoginViewModel loginViewModel = new();
            return View(loginViewModel);
        }
        [HttpPost]
        [AllowAnonymous]

        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            var user=await _loginService.Login(loginViewModel.UserName,loginViewModel.Password);
            if (user)
            {
                return RedirectToAction("Index", "User");

            }
            ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre yanlış.");
            return View(loginViewModel);
        }

        public async Task<IActionResult> Logout()
        {
             _loginService.Logout();

            return RedirectToAction("Login", "User");
        }
        public async Task<IActionResult> AccessDenied()
        {

            return View();
        }
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DetailForAdmin(int id)
        {
            var user = await _userService.Where(x => x.Id == id && !x.IsDeleted).FirstOrDefaultAsync();
            var accounts = await _accountService.Where(x => x.UserId == id && !x.IsDeleted).OrderByDescending(x => x.CreatedDate).ToListAsync();
            var userDetail = new UserDetailForAdminViewModel()
            {
                User = user,
                Accounts = accounts
            };


            return View(userDetail);
        }
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> List()
        {
            var users=await _userService.GetAllAsync();
            return View(users);
        }
    }
}
