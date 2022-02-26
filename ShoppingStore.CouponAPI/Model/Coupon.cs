using ShoppingStore.CouponAPI.Model.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingStore.CouponAPI.Model
{
    [Table("coupon")]
    public class Coupon : BaseEntity
    {
        [Column("coupon_code")]
        [Required]
        [StringLength(50)]
        public string CouponCode { get; set; }
        
        [Column("discount_amount")]
        [Required]
        public decimal DiscountAmount { get; set; }
       
    }
}
