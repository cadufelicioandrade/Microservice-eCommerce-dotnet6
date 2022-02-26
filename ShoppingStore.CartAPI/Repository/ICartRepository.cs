using ShoppingStore.CartAPI.Data.ValueObjects;

namespace ShoppingStore.CartAPI.Repository
{
    public interface ICartRepository
    {
        Task<CartVO> GetCartByUserId(string userId);
        Task<CartVO> SaveOrUpdateCart(CartVO vo);
        Task<bool> RemoveFromCart(long cartDetailsId);
        Task<bool> ApplyCoupon(string userId, string couponCode);
        Task<bool> RemoveCoupon(string userId);
        Task<bool> ClearCart(string userId);

    }
}
