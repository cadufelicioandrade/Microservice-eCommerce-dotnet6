using Newtonsoft.Json;
using ShoppingStore.Web.Models;
using ShoppingStore.Web.Services.Interfaces;
using ShoppingStore.Web.Utils;
using System.Net.Http.Headers;

namespace ShoppingStore.Web.Services
{
    public class CartService : ICartService
    {
        private readonly HttpClient _httpClient;
        public const string BasePath = "api/v1/cart";

        public CartService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<CartViewModel> GetCartByUserId(string userId, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _httpClient.GetAsync($"{BasePath}/find-cart/{userId}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Something went wrong when calling API: {response.ReasonPhrase}");

            return await response.ReadContentAs<CartViewModel>();
        }

        public async Task<CartViewModel> AddItemToCart(CartViewModel model, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var json = JsonConvert.SerializeObject(model);
            HttpResponseMessage response = await _httpClient.PostAsJson($"{BasePath}/add-cart", model);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Something went wrong when calling API: {response.ReasonPhrase}");

            return await response.ReadContentAs<CartViewModel>();
        }

        public async Task<CartViewModel> UpdateCart(CartViewModel cartViewModel, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _httpClient.PutAsJson($"{BasePath}/update-cart", cartViewModel);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Something went wrong when calling API: {response.ReasonPhrase}");

            return await response.ReadContentAs<CartViewModel>();
        }

        public async Task<bool> RemoveFromCart(long cartId, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{BasePath}/remove-cart/{cartId}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Something went wrong when calling API: {response.ReasonPhrase}");

            return await response.ReadContentAs<bool>();
        }

        public async Task<bool> ApplyCoupon(CartViewModel model, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _httpClient.PostAsJson($"{BasePath}/apply-coupon", model);
            var json = JsonConvert.SerializeObject(model);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Something went wrong when calling API: {response.ReasonPhrase}");

            return await response.ReadContentAs<bool>();
        }

        public async Task<bool> RemoveCoupon(string userId, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{BasePath}/remove-coupon/{userId}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Something went wrong when calling API: {response.ReasonPhrase}");

            return await response.ReadContentAs<bool>();
        }

        public async Task<bool> ClearCart(string userId, string token)
        {
            throw new NotImplementedException();
        }

        public async Task<object> Checkout(CartHeaderViewModel model, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _httpClient.PostAsJson($"{BasePath}/checkout", model);
            var json = JsonConvert.SerializeObject(model);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Something went wrong when calling API: {response.ReasonPhrase}");
            }
            else if(response.StatusCode.ToString().Equals("PreconditionFailed"))
            {
                return "Coupon Price has changed, please confirm!";
            }

            return await response.ReadContentAs<CartHeaderViewModel>();
        }
    }
}
