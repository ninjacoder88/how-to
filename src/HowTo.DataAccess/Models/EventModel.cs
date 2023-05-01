namespace HowTo.DataAccess.Models
{
    public sealed class EventModel
    {
        public string EventId { get; set; }

        public DateTimeOffset StartDateTime { get; set; }

        public DateTimeOffset EndDateTime { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string StartDate => StartDateTime.ToString("yyyy-MM-dd");

        public string StartTime => StartDateTime.ToString("HH:mm");

        public string EndDate => EndDateTime.ToString("yyyy-MM-dd");

        public string EndTime => EndDateTime.ToString("HH:mm");
    }
}
