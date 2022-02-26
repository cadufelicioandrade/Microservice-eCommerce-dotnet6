using AutoMapper;
using ShoppingStore.CouponAPI.Data.ValueObjects;
using ShoppingStore.CouponAPI.Model;

namespace ShoppingStore.CouponAPI.Config
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CouponVO, Coupon>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
