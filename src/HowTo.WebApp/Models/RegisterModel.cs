namespace HowTo.WebApp.Models
{
    public sealed class RegisterModel
    {
        public RegisterModel(string apiUrl, string successUrl)
        {
            ApiUrl = apiUrl;
            SuccessUrl = successUrl;
        }

        public string ApiUrl { get; }

        public string SuccessUrl { get; }
    }
}
