namespace HowTo.WebApp.Models
{
    public sealed class LoginModel
    {
        public LoginModel(string apiUrl, string successUrl)
        {
            ApiUrl = apiUrl;
            SuccessUrl = successUrl;
        }

        public string ApiUrl { get; }

        public string SuccessUrl { get; }
    }
}
