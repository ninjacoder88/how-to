using HowTo.DataAccess.Entities;
using HowTo.DataAccess.Extensions;
using HowTo.DataAccess.Models;
using HowTo.WebApi.Extensions;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HowTo.DataAccess
{
    public interface IRepository
    {
        //Task CreateCustomerAsync(CustomerEntity entity);

        //Task<CustomerEntity?> LoadCustomerAsync(string id);

        Task<string> CreateUserAsync(string username, string hashedPassword, string emailAddress, byte[] salt);

        Task<UserModel?> LoadUserByUsernameOrEmailAddressAsync(string username, string emailAddress);

        Task<LoginUserModel?> LoadUserByUsernameAsync(string username);

        Task<LoginUserModel?> LoadUserByUserIdAsync(string userId);
    }

    public sealed class Repository : IRepository
    {
        public Repository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("HowTo");
            _mongoClient = new MongoClient(connectionString);
        }

        //public async Task CreateCustomerAsync(CustomerEntity entity) => await GetCustomerCollection().InsertOneAsync(entity);

        //public async Task<CustomerEntity?> LoadCustomerAsync(string id)
        //{
        //    if(!ObjectId.TryParse(id, out ObjectId customerId))
        //        return null;
        //    var collection = GetCustomerCollection();
        //    var cursor = await collection.FindAsync(x => x._id == customerId);
        //    var list = await cursor.ToListAsync();
        //    var item = list.SingleOrDefault();
        //    if (item == null)
        //        return null;
        //    return item;
        //}

        public async Task<string> CreateUserAsync(string username, string hashedPassword, string emailAddress, byte[] salt)
        {
            var entity = new UserEntity
            {
                Username = username,
                EmailAddress = emailAddress,
                HashedPassword = hashedPassword,
                Salt = salt
            };
            await GetUserCollection().InsertOneAsync(entity);
            return entity._id.ToString();
        }

        public async Task<UserModel?> LoadUserByUsernameOrEmailAddressAsync(string username, string emailAddress)
        {
            var entity = await GetUserCollection().FindOneAsync(f => f.Username == username || f.EmailAddress == emailAddress);
            if (entity == null)
                return null;
            return entity.ToModel();
        }

        public async Task<LoginUserModel?> LoadUserByUsernameAsync(string username)
        {
            var entity = await GetUserCollection().FindOneAsync(f => f.Username == username);
            if (entity == null)
                return null;
            return entity.ToLoginModel();
        }

        public async Task<LoginUserModel?> LoadUserByUserIdAsync(string userId)
        {
            if (!ObjectId.TryParse(userId, out ObjectId objectId))
                return null;

            var entity = await GetUserCollection().FindOneAsync(f => f._id == objectId);
            if (entity == null)
                return null;

            return entity.ToLoginModel();
        }

        private IMongoDatabase GetDatabase() => _mongoClient.GetDatabase("HowTo");
        //private IMongoCollection<CustomerEntity> GetCustomerCollection() => GetDatabase().GetCollection<CustomerEntity>("Customers");
        private IMongoCollection<UserEntity> GetUserCollection() => GetDatabase().GetCollection<UserEntity>("Users");

        private readonly IMongoClient _mongoClient;
    }
}
