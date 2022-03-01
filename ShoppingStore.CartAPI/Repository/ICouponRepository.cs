using ShoppingStore.CartAPI.Data.ValueObjects;

namespace ShoppingStore.CartAPI.Repository
{
    public interface ICouponRepository
    {
        Task<CouponVO> GetCoupon(string couponCode, string token);
    }
}
