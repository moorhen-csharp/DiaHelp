using DiaHelp.Model;
using System.Net.Http.Json;

namespace DiaHelp.Services
{
    public class AuthService
    {
        private readonly string ClientId = "a13c6e23bebf4cc490e294a57e99cff6";
        private readonly string ClientSecret = "0f2c96fec93e40f6afbedca6feba7674";
        private readonly string RedirectUri = "diahelp:/oauth2redirect";
        private readonly HttpClient _httpClient = new HttpClient();

        public string GetLoginUrl()
        {
            return $"https://oauth.yandex.ru/authorize?response_type=code&client_id={ClientId}&redirect_uri={Uri.EscapeDataString(RedirectUri)}&scope=login:email login:info";
        }

        public async Task<AuthResponse> GetTokenAsync(string code)
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "code", code },
                { "client_id", ClientId },
                { "client_secret", ClientSecret }
            });

            var response = await _httpClient.PostAsync("https://oauth.yandex.ru/token", content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<AuthResponse>();
        }
    }

}
