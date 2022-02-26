using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShoppingStore.CouponAPI.Data.ValueObjects;
using ShoppingStore.CouponAPI.Model.Context;

namespace ShoppingStore.CouponAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly MySQLContext _context;
        private readonly IMapper _mapper;

        public CouponRepository(MySQLContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CouponVO> GetCouponByCouponCode(string couponCode)
        {
            var coupon = await _context.Coupons.FirstOrDefaultAsync( c => c.CouponCode == couponCode);
            return _mapper.Map<CouponVO>(coupon);
        }
    }
}
