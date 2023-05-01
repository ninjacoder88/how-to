namespace HowTo.WebApi.Models
{
    public class CreateEventModel
    {
        public string StartDate { get; set; }

        public string StartTime { get; set; }

        public string EndDate { get; set; }

        public string EndTime { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
