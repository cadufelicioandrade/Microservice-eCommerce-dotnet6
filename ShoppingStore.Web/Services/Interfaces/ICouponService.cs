using ShoppingStore.Web.Models;

namespace ShoppingStore.Web.Services.Interfaces
{
    public interface ICouponService
    {
        Task<CouponViewModel> GetCoupon(string couponCode, string token);
    }
}
