using Microsoft.AspNetCore.Mvc;

namespace AccountExample.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
