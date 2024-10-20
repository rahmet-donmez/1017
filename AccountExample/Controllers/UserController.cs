using AccountManagment.Core.Models;
using AccountManagment.Core.Repositories;
using AccountManagment.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccountExample.Controllers
{
    public class UserController : Controller
    {
        private readonly IGenericService<User> _service;

        public UserController(IGenericService<User> service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var user=await _service.GetByIdAsync(1);
            return View(user);
        }
    }
}
