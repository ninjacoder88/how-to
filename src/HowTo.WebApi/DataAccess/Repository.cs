using HowTo.WebApi.DataAccess.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HowTo.WebApi.DataAccess
{
    public interface IRepository
    {
        Task CreateCustomerAsync(CustomerEntity entity);

        Task<CustomerEntity?> LoadCustomerAsync(string id);
    }

    internal sealed class Repository : IRepository
    {
        public Repository(string connectionString)
        {
            _mongoClient = new MongoClient(connectionString);
        }

        public async Task CreateCustomerAsync(CustomerEntity entity)
        {
            var collection = GetCustomerCollection();
            await collection.InsertOneAsync(entity);
        }

        public async Task<CustomerEntity?> LoadCustomerAsync(string id)
        {
            if(!ObjectId.TryParse(id, out ObjectId customerId))
                return null;
            var collection = GetCustomerCollection();
            var cursor = await collection.FindAsync(x => x._id == customerId);
            var list = await cursor.ToListAsync();
            var item = list.SingleOrDefault();
            if (item == null)
                return null;
            return item;
        }

        private IMongoDatabase GetDatabase() => _mongoClient.GetDatabase("HowTo");
        private IMongoCollection<CustomerEntity> GetCustomerCollection() => GetDatabase().GetCollection<CustomerEntity>("Customers");

        private readonly IMongoClient _mongoClient;
    }
}
