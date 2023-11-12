using Newtonsoft.Json;
using System.Text;
using UI.FE.Models;

namespace UI.FE.Data
{
    public class BasketHttpClient : IBasketHttpClient
    {
        private readonly ILogger<BasketHttpClient> _logger;
        private readonly HttpClient _httpClient;

        public BasketHttpClient(ILogger<BasketHttpClient> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<CartResponse?> GetCartByUserIdAsync(string userId)
        {
            string endpoint = "/api/Baskets/GetCartByUserId/" + userId;

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    CartResponse? cart = JsonConvert.DeserializeObject<CartResponse>(responseBody);

                    if (cart is null)
                    {
                        _logger.LogError($"There is no data for it");
                        return null;
                    }


                    return cart;
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

        public async Task AddToCart(CartItemAddingRequest request)
        {
            string endpoint = "/api/Baskets/AddToCart";

            try
            {
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(endpoint, content);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Request error: {ex.Message}");
            } 
        }
    }
}
