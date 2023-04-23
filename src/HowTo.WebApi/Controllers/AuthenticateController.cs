﻿using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace HowTo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        public AuthenticateController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string username, [FromQuery] string password, [FromQuery] string clientId, [FromQuery] string clientSecret)
        {
            var identityApi = _configuration.GetValue<string>("ServiceConnections:IdentityApiInternal");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(identityApi);
                var response = await client.PostAsync("connect/token", new StringContent($"grant_type=password&username={username}&password={password}&client_id={clientId}&client_secret={clientSecret}&scope=WebAppScope", Encoding.UTF8, "application/x-www-form-urlencoded"));
                var responseContent = await response.Content.ReadAsStringAsync();
                return new JsonResult(responseContent);
            }
        }

        private readonly IConfiguration _configuration;
    }
}
