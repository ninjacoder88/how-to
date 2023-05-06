using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace HowTo.Client
{
    internal sealed class Runner
    {
        public async Task GetDataAsync()
        {
            var tokenResponse = await GetTokenAsync();

            var eventsResponse = await GetEventsAsync(tokenResponse.access_token);
        }

        private async Task<IdentityTokenResponse> GetTokenAsync()
        {
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8092/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var form = new Dictionary<string, string>();
                form.Add("grant_type", "password");
                form.Add("client_id", "webapplogin");
                form.Add("client_secret", "7c6e005b-aaf7-425a-a8c4-f488ff5dd95b");
                form.Add("username", "testaccount");
                form.Add("password", "password");
                form.Add("scope", "WebAppScope");

                var response = await client.PostAsync("connect/token", new FormUrlEncodedContent(form));
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new Exception(responseContent);

                Console.WriteLine(responseContent);
                Console.WriteLine();

                return JsonConvert.DeserializeObject<IdentityTokenResponse>(responseContent);
            }
        }

        private async Task<string> GetEventsAsync(string token)
        {
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8091/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync("api/Events");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new Exception(responseContent);

                Console.WriteLine(responseContent);

                return responseContent;
            }
        }
    }
}
