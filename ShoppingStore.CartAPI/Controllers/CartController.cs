using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingStore.CartAPI.Data.ValueObjects;
using ShoppingStore.CartAPI.Messages;
using ShoppingStore.CartAPI.RabbitMQSender;
using ShoppingStore.CartAPI.Repository;

namespace ShoppingStore.CartAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ILogger<CartController> _logger;
        private ICartRepository _cartRepository;
        private ICouponRepository _couponRepository;
        private IRabbitMQMessageSender _rabbitMQMessageSender;

        public CartController(ICartRepository productRepository,
                            ILogger<CartController> logger,
                            IRabbitMQMessageSender rabbitMQMessageSender, 
                            ICouponRepository couponRepository)
        {
            _cartRepository = productRepository ??
                                    throw new ArgumentNullException(nameof(productRepository));
            _logger = logger;
            _rabbitMQMessageSender = rabbitMQMessageSender;
            _couponRepository = couponRepository;
        }

        [HttpGet("find-cart/{id}")]
        public async Task<ActionResult<CartVO>> GetCartByUserId(string id)
        {
            var cart = await _cartRepository.GetCartByUserId(id);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        [HttpPost("add-cart")]
        public async Task<ActionResult<CartVO>> AddCart([FromBody] CartVO vo)
        {
            var cart = await _cartRepository.SaveOrUpdateCart(vo);
            if (cart == null) return NotFound();
            return Ok(cart);
        }


        [HttpPut("update-cart")]
        public async Task<ActionResult<CartVO>> UpdateCart([FromBody] CartVO vo)
        {
            var cart = await _cartRepository.SaveOrUpdateCart(vo);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        [HttpDelete("remove-cart/{id}")]
        public async Task<ActionResult<CartVO>> RemoveCart(int id)
        {
            var status = await _cartRepository.RemoveFromCart(id);
            if (status == null) return NotFound(status);
            return Ok(status);
        }


        [HttpPost("apply-coupon")]
        public async Task<ActionResult<CartVO>> ApplyCoupon([FromBody] CartVO vo)
        {
            var status = await _cartRepository.ApplyCoupon(vo.CartHeader.UserId, vo.CartHeader.CouponCode);
            if (!status) return NotFound(status);
            return Ok(status);
        }
        
        [HttpDelete("remove-coupon/{userId}")]
        public async Task<ActionResult<CartVO>> RemoveCoupon(string userId)
        {
            var status = await _cartRepository.RemoveCoupon(userId);
            if (!status) return NotFound(status);
            return Ok(status);
        }
        
        [HttpPost("checkout")]
        public async Task<ActionResult<CheckoutHeaderVO>> Checkout([FromBody] CheckoutHeaderVO vo)
        {
            string token = Request.Headers["Authorization"];
            if (vo?.UserId == null) 
                return BadRequest();

            var cart = await _cartRepository.GetCartByUserId(vo.UserId);

            if (cart == null) 
                return NotFound();

            if (!String.IsNullOrEmpty(vo.CouponCode))
            {
                CouponVO coupon = await _couponRepository.GetCoupon(vo.CouponCode, token);
                if(vo.DiscountAmount != coupon.DiscountAmount)
                {
                    return StatusCode(412);
                }
            }

            vo.CartDetails = cart.CartDetails;
            vo.DateTime = DateTime.Now;

            _rabbitMQMessageSender.SendMessage(vo, "checkoutqueue");

            return Ok(vo);
        }

    }
}