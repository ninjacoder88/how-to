using MongoDB.Driver;
using System.Linq.Expressions;

namespace HowTo.DataAccess.Extensions
{
    internal static class MongoCollectionExtensions
    {
        public static async Task<T?> FindOneAsync<T>(this IMongoCollection<T> collection, Expression<Func<T, bool>> filter)
        {
            var cursor = await collection.FindAsync(filter);
            var list = await cursor.ToListAsync();

            if (list.Count != 1)
                return default;
            return list[0];
        }
    }
}
