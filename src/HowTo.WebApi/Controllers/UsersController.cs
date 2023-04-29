﻿using HowTo.DataAccess;
using HowTo.Utility;
using HowTo.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace HowTo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public UsersController(IRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
        {
            if (model == null)
                return BadRequest("No data received");

            if (string.IsNullOrEmpty(model.Username))
                return BadRequest("Username must have a value");

            if (string.IsNullOrEmpty(model.Password))
                return BadRequest("Password must have a value");

            //todo: check password length

            var formattedUsername = model.Username.Trim().ToLower();

            var user = await _repository.LoadUserByUsernameAsync(formattedUsername);

            if(user == null)
                return NotFound("Username does not exist");

            var hashedPassword = new Encryptor().HashEncrypt(model.Password, user.Salt);

            //todo: set login failed count
            if (user.HashedPassword != hashedPassword)
                return Unauthorized("Invalid password");

            //todo: set last login time
            var identityApi = _configuration.GetValue<string>("ServiceConnections:IdentityApiInternal");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(identityApi);
                var response = await client.PostAsync("connect/token", new StringContent($"grant_type=password&username={formattedUsername}&password={model.Password}&client_id=webapplogin&client_secret=7c6e005b-aaf7-425a-a8c4-f488ff5dd95b&scope=WebAppScope", Encoding.UTF8, "application/x-www-form-urlencoded"));
                var responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                    return new JsonResult(new ResponseModel<string>(false) { ErrorMessage = responseContent });
                return new JsonResult(new ResponseModel<TokenModel>(true) { Data = JsonConvert.DeserializeObject<TokenModel>(responseContent) });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] CreateUserModel model)
        {
            if(model == null)
                return BadRequest("No data received");

            if (string.IsNullOrEmpty(model.Username))
                return BadRequest("Username must have a value");

            if (string.IsNullOrEmpty(model.Password))
                return BadRequest("Password must have a value");

            if (string.IsNullOrEmpty(model.EmailAddress))
                return BadRequest("Email Address must have a value");

            //todo: check password length min 8, max 100
            //todo: ensure email matches pattern

            var formattedUsername = model.Username.Trim().ToLower();

            var user = await _repository.LoadUserByUsernameOrEmailAddressAsync(formattedUsername, model.EmailAddress);

            if (user != null)
            {
                if (user.Username == formattedUsername)
                    return BadRequest("Username is already in use");

                if (user.EmailAddress == model.EmailAddress)
                    return BadRequest("Email Address has already been registered");

                return new ObjectResult("An unknow error occurred while verifying username and email address") { StatusCode = (int)HttpStatusCode.InternalServerError };
            }

            byte[] salt = RandomNumberGenerator.GetBytes(16);
            var hashedPassword = new Encryptor().HashEncrypt(model.Password, salt);

            var userId = await _repository.CreateUserAsync(formattedUsername, hashedPassword, model.EmailAddress, salt);
            return new JsonResult(new ResponseModel<string>(true) { Data = userId });
        }

        private readonly IRepository _repository;
        private readonly IConfiguration _configuration;
    }
}
