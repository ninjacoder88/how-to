using MongoDB.Bson;

namespace HowTo.DataAccess.Entities
{
    internal sealed class UserEntity
    {
        public ObjectId _id { get; set; }

        public string Username { get; set; }

        public string HashedPassword { get; set; }

        public byte[] Salt { get; set; }

        public string EmailAddress { get; set; }

        public DateTimeOffset LastLoginTime { get; set; }

        public int FailedLoginCount { get; set; }

        public int UserStatusId { get; set; }
    }
}
