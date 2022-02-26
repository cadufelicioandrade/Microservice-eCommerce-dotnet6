using ShoppingStore.Web.Models;
using ShoppingStore.Web.Services.Interfaces;
using ShoppingStore.Web.Utils;
using System.Net.Http.Headers;

namespace ShoppingStore.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        public const string BasePath = "api/v1/product";

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<IEnumerable<ProductViewModel>> GetAllProduct(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _httpClient.GetAsync(BasePath);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Something went wrong when calling API: {response.ReasonPhrase}");

            return await response.ReadContentAs<List<ProductViewModel>>();
        }

        public async Task<ProductViewModel> GetProductById(long productId, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _httpClient.GetAsync($"{BasePath}/{productId}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Something went wrong when calling API: {response.ReasonPhrase}");

            return await response.ReadContentAs<ProductViewModel>();
        }

        public async Task<ProductViewModel> CreateProduct(ProductViewModel model, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _httpClient.PostAsJson(BasePath, model);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Something went wrong when calling API: {response.ReasonPhrase}");

            return await response.ReadContentAs<ProductViewModel>();

        }

        public async Task<ProductViewModel> UpdateProduct(ProductViewModel model, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _httpClient.PutAsJson(BasePath, model);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Something went wrong when calling API: {response.ReasonPhrase}");

            return await response.ReadContentAs<ProductViewModel>();
        }

        public async Task<bool> DeleteProductId(long productId, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{BasePath}/{productId}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Something went wrong when calling API: {response.ReasonPhrase}");

            return await response.ReadContentAs<bool>();
        }

    }
}
