namespace HowTo.WebApp.Models
{
    public sealed class HomeModel
    {
        public HomeModel(string apiUrl)
        {
            ApiUrl = apiUrl;
        }

        public string ApiUrl { get; }
    }
}
