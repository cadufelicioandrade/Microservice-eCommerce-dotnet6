using Microsoft.EntityFrameworkCore;
using ShoppingStore.OrderAPI.Model;
using ShoppingStore.OrderAPI.Model.Context;

namespace ShoppingStore.OrderAPI.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbContextOptions<MySQLContext> _contextOptions;

        public OrderRepository(DbContextOptions<MySQLContext> contextOptions)
        {
            _contextOptions = contextOptions;
        }

        public async Task<bool> AddOrder(OrderHeader header)
        {
            if (header is null)
                return false;
            
            await using var context = new MySQLContext(_contextOptions);
            context.Headers.Add(header);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task UpdateOrderPaymentStatus(long orderHeaderId, bool status)
        {
            await using var context = new MySQLContext(_contextOptions);
            var header = await context.Headers.FirstOrDefaultAsync(o => o.Id == orderHeaderId);

            if(header != null)
            {
                header.PaymentStatus = status;
                await context.SaveChangesAsync();
            }

        }
    }
}
