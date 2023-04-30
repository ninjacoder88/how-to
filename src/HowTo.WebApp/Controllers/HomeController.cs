using HowTo.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace HowTo.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index() => View();

        [HttpGet]
        public IActionResult Login() => View(new LoginModel(_configuration.GetValue<string>("ServiceConnections:ApiUrl"), Url.Action("Index", "Events")));

        [HttpGet]
        public IActionResult Register() => View(new RegisterModel(_configuration.GetValue<string>("ServiceConnections:ApiUrl"), Url.Action("Login")));

        private readonly IConfiguration _configuration;
    }
}
