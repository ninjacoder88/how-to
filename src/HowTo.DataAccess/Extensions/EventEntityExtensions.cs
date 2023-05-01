using HowTo.DataAccess.Entities;
using HowTo.DataAccess.Models;

namespace HowTo.DataAccess.Extensions
{
    internal static class EventEntityExtensions
    {
        public static EventModel ToModel(this EventEntity entity)
        {
            return new EventModel
            {
                Description = entity.Description,
                EndDateTime = entity.EndDateTime,
                EventId = entity._id.ToString(),
                StartDateTime = entity.StartDateTime,
                Title = entity.Title,
            };
        }
    }
}
