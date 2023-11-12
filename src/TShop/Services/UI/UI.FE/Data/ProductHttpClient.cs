using Newtonsoft.Json;
using UI.FE.ResponseDto;

namespace UI.FE.Data
{
    public class ProductHttpClient : IProductHttpClient
    {
        private readonly ILogger<ProductHttpClient> _logger;
        private readonly HttpClient _httpClient;

        public ProductHttpClient(ILogger<ProductHttpClient> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Product>?> GetAllProductsAsync()
        {
            string endpoint = "/api/Catalog";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    IEnumerable<Product>? products = JsonConvert.DeserializeObject<IEnumerable<Product>>(responseBody);

                    if (products is null)
                    {
                        _logger.LogError($"There is no data for it");
                        return null;
                    }


                    return products;
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
