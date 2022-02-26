using Microsoft.EntityFrameworkCore;

namespace ShoppingStore.OrderAPI.Model.Context
{
    public class MySQLContext : DbContext
    {
        public MySQLContext(){}
        public MySQLContext(DbContextOptions<MySQLContext> options) : base(options){}

        public DbSet<OrderDetail> Details { get; set; }
        public DbSet<OrderHeader> Headers { get; set; }

    }
}
