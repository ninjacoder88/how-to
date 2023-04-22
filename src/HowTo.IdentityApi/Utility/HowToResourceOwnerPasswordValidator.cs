using HowTo.IdentityApi.DataAccess;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System.Security.Claims;

namespace HowTo.IdentityApi
{
    internal sealed class HowToResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public HowToResourceOwnerPasswordValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await _userRepository.LoadUserAsync(context.UserName);

            if(user == null)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Username or password is invalid");
                return;
            }

            // password on the user object is ideally hashed so there may be a need to hash the incoming password
            if(user.Password != context.Password)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Username or password is invalid");
                return;
            }

            var now = DateTime.UtcNow;

            // this should only be the required claims. specific claims should be set in the profile service
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Id, user.Id.ToString()),
                new Claim(JwtClaimTypes.Subject, user.Username),
                new Claim(JwtClaimTypes.AuthenticationTime, now.ToEpochTime().ToString()),
                new Claim(JwtClaimTypes.IdentityProvider, "howto"),
            };

            context.Result = new GrantValidationResult { Subject = new ClaimsPrincipal(new ClaimsIdentity(claims)) };
        }

        private readonly IUserRepository _userRepository;
    }
}
