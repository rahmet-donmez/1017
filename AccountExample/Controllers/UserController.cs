using Microsoft.AspNetCore.Mvc;

namespace AccountExample.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
