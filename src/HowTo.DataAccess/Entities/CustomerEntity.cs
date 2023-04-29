using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HowTo.DataAccess.Entities
{
    [BsonIgnoreExtraElements]
    internal sealed class CustomerEntity
    {
        public ObjectId _id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public List<AddressEntity> Addresses { get; set; }
    }

    internal sealed class AddressEntity
    {
        public string StreetAddress { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string AddressType { get; set; }

        public bool IsPrimary { get; set; }
    }
}
