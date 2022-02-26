using ShoppingStore.Web.Models;

namespace ShoppingStore.Web.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartViewModel> GetCartByUserId(string userId, string token);
        Task<CartViewModel> AddItemToCart(CartViewModel cartViewModel, string token);
        Task<CartViewModel> UpdateCart(CartViewModel cartViewModel, string token);
        Task<bool> RemoveFromCart(long cartId, string token);

        Task<bool> ApplyCoupon(CartViewModel cartViewModel, string token);
        Task<bool> RemoveCoupon(string userId, string token);
        Task<bool> ClearCart(string userId, string token);
        Task<CartHeaderViewModel> Checkout(CartHeaderViewModel model, string token);

    }
}
