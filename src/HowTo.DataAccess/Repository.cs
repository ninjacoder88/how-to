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
        Task<string> CreateUserAsync(string username, string hashedPassword, string emailAddress, byte[] salt);

        Task<UserModel?> LoadUserByUsernameOrEmailAddressAsync(string username, string emailAddress);

        Task<LoginUserModel?> LoadUserByUsernameAsync(string username);

        Task<LoginUserModel?> LoadUserByUserIdAsync(string userId);

        Task UpdateFailedLoginCountAsync(string userId);

        Task UpdateLastLoginAsync(string userId);

        Task<string> CreateEventAsync(string title, string description, DateTimeOffset start, DateTimeOffset end);

        Task<List<EventModel>> LoadEventsAsync(DateTimeOffset start, DateTimeOffset end);

        Task<EventModel?> LoadEventAsync(string eventId);
    }

    public sealed class Repository : IRepository
    {
        public Repository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("HowTo");
            _mongoClient = new MongoClient(connectionString);
        }

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

        public async Task UpdateFailedLoginCountAsync(string userId)
        {
            if (!ObjectId.TryParse(userId, out ObjectId objectId))
                return;

            var user = await GetUserCollection().FindOneAsync(f => f._id == objectId);
            if (user == null)
                return;

            UpdateDefinition<UserEntity> updateDefinition =
                new UpdateDefinitionBuilder<UserEntity>()
                .Set(t => t.FailedLoginCount, user.FailedLoginCount);

            await GetUserCollection().FindOneAndUpdateAsync(f => f._id == objectId, updateDefinition);
        }

        public async Task UpdateLastLoginAsync(string userId)
        {
            if (!ObjectId.TryParse(userId, out ObjectId objectId))
                return;

            var user = await GetUserCollection().FindOneAsync(f => f._id == objectId);
            if (user == null)
                return;

            UpdateDefinition<UserEntity> updateDefinition =
                new UpdateDefinitionBuilder<UserEntity>()
                .Set(t => t.FailedLoginCount, 0)
                .Set(t => t.LastLoginTime, DateTimeOffset.Now);

            await GetUserCollection().FindOneAndUpdateAsync(f => f._id == objectId, updateDefinition);
        }

        public async Task<string> CreateEventAsync(string title, string description, DateTimeOffset start, DateTimeOffset end)
        {
            var entity = new EventEntity
            {
                Description = description,
                EndDateTime = end,
                StartDateTime = start,
                Title = title
            };
            await GetEventCollection().InsertOneAsync(entity);
            return entity._id.ToString();
        }

        public async Task<List<EventModel>> LoadEventsAsync(DateTimeOffset start, DateTimeOffset end)
        {
            var cursor = await GetEventCollection().FindAsync(f => f.StartDateTime >= start || f.EndDateTime <= end);
            var list = await cursor.ToListAsync();
            return list.Select(x => x.ToModel()).ToList();
        }

        public async Task<EventModel?> LoadEventAsync(string eventId)
        {
            if (!ObjectId.TryParse(eventId, out var objectId))
                return null;
            
            var entity = await GetEventCollection().FindOneAsync(f => f._id == objectId);
            return entity?.ToModel();
        }

        private IMongoDatabase GetDatabase() => _mongoClient.GetDatabase("HowTo");
        private IMongoCollection<UserEntity> GetUserCollection() => GetDatabase().GetCollection<UserEntity>("Users");
        private IMongoCollection<EventEntity> GetEventCollection() => GetDatabase().GetCollection<EventEntity>("Events");

        private readonly IMongoClient _mongoClient;
    }
}
