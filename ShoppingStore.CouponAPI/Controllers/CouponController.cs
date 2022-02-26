using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingStore.CouponAPI.Data.ValueObjects;
using ShoppingStore.CouponAPI.Repository;

namespace ShoppingStore.CouponAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CouponController : ControllerBase
    {
        private ICouponRepository _couponRepository;
        private readonly ILogger<CouponController> _logger;

        public CouponController(ILogger<CouponController> logger, 
                                        ICouponRepository couponRepository = null)
        {
            _logger = logger;
            _couponRepository = couponRepository ?? throw new ArgumentException(nameof(couponRepository));
        }

        [HttpGet("{couponCode}")]
        [Authorize]
        public async Task<ActionResult<CouponVO>> GetCouponByCouponCode(string couponCode)
        {
            var couponVO = await _couponRepository.GetCouponByCouponCode(couponCode);
            if(couponVO == null) return NotFound();
            return Ok(couponVO);
        }
    }
}