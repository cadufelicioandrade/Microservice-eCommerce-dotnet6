using ShoppingStore.Web.Models;
using ShoppingStore.Web.Services.Interfaces;
using ShoppingStore.Web.Utils;
using System.Net;
using System.Net.Http.Headers;

namespace ShoppingStore.Web.Services
{
    public class CouponService : ICouponService
    {
        private readonly HttpClient _httpClient;
        public const string BasePath = "api/v1/coupon";

        public CouponService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }


        public async Task<CouponViewModel> GetCoupon(string couponCode, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _httpClient.GetAsync($"{BasePath}/{couponCode}");

            if (response.StatusCode != HttpStatusCode.OK)
                return new CouponViewModel();

            return await response.ReadContentAs<CouponViewModel>();
        }
    }
}
