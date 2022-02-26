using ShoppingStore.OrderAPI.Model;

namespace ShoppingStore.OrderAPI.Repository
{
    public interface IOrderRepository
    {
        Task<bool> AddOrder(OrderHeader header);
        Task UpdateOrderPaymentStatus(long orderHeaderId, bool paid);

    }
}
