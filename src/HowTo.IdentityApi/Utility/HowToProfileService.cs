using HowTo.DataAccess;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Security.Claims;

namespace HowTo.IdentityApi
{
    internal sealed class HowToProfileService : IProfileService
    {
        public HowToProfileService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var idClaim = context.Subject.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Id);

            if (idClaim == null)
                return;

            if (string.IsNullOrEmpty(idClaim.Value))
                return;

            //if (!int.TryParse(idClaim.Value, out var id))
            //    return;

            var model = await _repository.LoadUserByUserIdAsync(idClaim.Value);

            if (model == null)
                return;

            // add claims for this user
            context.IssuedClaims = new List<Claim>
            {
                new Claim("customclaim", "something"),
                new Claim(JwtClaimTypes.Role, "user"),
                //new Claim(JwtClaimTypes.Email, user.EmailAddress)
            };
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            // additional logic to determine if the user is active can go here, but generally just setting the IsActive flag to true is fine
            await Task.Delay(100);
            context.IsActive = true;
        }

        private readonly IRepository _repository;
    }
}
