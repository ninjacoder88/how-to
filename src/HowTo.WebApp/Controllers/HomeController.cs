using Microsoft.AspNetCore.Mvc;

namespace HowTo.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
