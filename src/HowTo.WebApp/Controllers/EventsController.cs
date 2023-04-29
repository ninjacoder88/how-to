using Microsoft.AspNetCore.Mvc;

namespace HowTo.WebApp.Controllers
{
    public class EventsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
