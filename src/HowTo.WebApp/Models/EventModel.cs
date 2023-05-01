namespace HowTo.WebApp.Models
{
    public sealed class EventModel
    {
        public EventModel(string apiUrl)
        {
            ApiUrl = apiUrl;
        }

        public string ApiUrl { get; }
    }
}
