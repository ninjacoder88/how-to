using HowTo.DataAccess;
using HowTo.Utility;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System.Security.Claims;

namespace HowTo.IdentityApi
{
    internal sealed class HowToResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public HowToResourceOwnerPasswordValidator(IRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var model = await _repository.LoadUserByUsernameAsync(context.UserName);

            if(model == null)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Username or password is invalid");
                return;
            }

            var hashedPassword = new Encryptor().HashEncrypt(context.Password, model.Salt);
            // password on the user object is ideally hashed so there may be a need to hash the incoming password
            if (model.HashedPassword != hashedPassword)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Username or password is invalid");
                return;
            }

            var now = DateTime.UtcNow;

            // this should only be the required claims. specific claims should be set in the profile service
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Id, model.UserId),
                new Claim(JwtClaimTypes.Subject, model.Username),
                new Claim(JwtClaimTypes.AuthenticationTime, now.ToEpochTime().ToString()),
                new Claim(JwtClaimTypes.IdentityProvider, _configuration.GetValue<string>("Issuer")),
            };

            context.Result = new GrantValidationResult { Subject = new ClaimsPrincipal(new ClaimsIdentity(claims)) };
        }

        private readonly IRepository _repository;
        private readonly IConfiguration _configuration;
    }
}
