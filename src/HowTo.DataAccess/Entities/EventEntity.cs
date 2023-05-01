using MongoDB.Bson;

namespace HowTo.DataAccess.Entities
{
    internal class EventEntity
    {
        public ObjectId _id { get; set; }

        public DateTimeOffset StartDateTime { get; set; }

        public DateTimeOffset EndDateTime { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
