using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace HowTo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    [RequiredScope("WebAppScope")]
    public class AdminController : ControllerBase
    {
        public AdminController(IWebHostEnvironment environment, IConfiguration configuration)
        {
            _environment = environment;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await Task.Delay(100);
            return new JsonResult(new
            {
                _environment.EnvironmentName,
                IdentityApiPublic = _configuration.GetValue<string>("ServiceConnections:IdentityApiPublic"),
                IdentityApiInternal = _configuration.GetValue<string>("ServiceConnections:IdentityApiInternal"),
                Claims = HttpContext.User.Claims.Select(x => new { x.Issuer, x.Type, x.Value, x.ValueType })
            });
        }

        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
    }
}
