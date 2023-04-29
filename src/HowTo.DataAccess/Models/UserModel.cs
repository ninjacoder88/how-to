using HowTo.DataAccess.Entities;
using MongoDB.Bson;

namespace HowTo.DataAccess.Models
{
    public sealed class UserModel
    {
        public string? UserId { get; set; }

        public string Username { get; set; }

        public string EmailAddress { get; set; }
    }

    internal static class UserModelExtensions
    {
        public static UserEntity ToEntity(this UserModel model)
        {
            var entity = new UserEntity
            {
                EmailAddress = model.EmailAddress,
                Username = model.Username,
            };

            if(model.UserId != null)
                entity._id = ObjectId.Parse(model.UserId);

            return entity;
        }
    }
}
