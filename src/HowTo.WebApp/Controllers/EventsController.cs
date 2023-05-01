using HowTo.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace HowTo.WebApp.Controllers
{
    public class EventsController : Controller
    {
        public EventsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View(new EventModel(_configuration.GetValue<string>("ServiceConnections:ApiUrl")));
        }

        private readonly IConfiguration _configuration;
    }
}
