using Newtonsoft.Json;
using System.Text;
using UI.FE.Models;

namespace UI.FE.Data
{
    public class AuthenticateHttpClient : IAuthenticateHttpClient
    {
        private readonly ILogger<AuthenticateHttpClient> _logger;
        private readonly HttpClient _httpClient;

        public AuthenticateHttpClient(ILogger<AuthenticateHttpClient> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            string endpoint = "/api/Authenticate/Login";

            try
            {
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(endpoint, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    LoginResponse? loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseBody);

                    if (loginResponse is null)
                    {
                        _logger.LogError($"There is an issue while login");
                        return null;
                    }


                    return loginResponse;
                }
                else
                {
                    _logger.LogError($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Request error: {ex.Message}");
            }

            return null;
        }
    }
}
