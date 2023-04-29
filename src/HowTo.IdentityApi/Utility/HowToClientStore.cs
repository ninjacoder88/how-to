using HowTo.DataAccess;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace HowTo.IdentityApi.Utility
{
    internal class HowToClientStore : IClientStore
    {
        public HowToClientStore(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<Client?> FindClientByIdAsync(string clientId)
        {
            var user = await _repository.LoadUserByUserIdAsync(clientId);

            if(user == null)
                return null;

            return new Client
            {
                ClientId = clientId,
            };
        }

        private readonly IRepository _repository;
    }
}
