using ShoppingStore.CartAPI.Model;

namespace ShoppingStore.CartAPI.Data.ValueObjects
{
    public class CartVO
    {
        public CartHeaderVO CartHeader { get; set; }
        public IEnumerable<CartDetailVO>? CartDetails { get; set; }

    }
}
