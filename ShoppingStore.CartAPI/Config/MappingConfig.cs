using AutoMapper;
using ShoppingStore.CartAPI.Data.ValueObjects;
using ShoppingStore.CartAPI.Model;

namespace ShoppingStore.CartAPI.Config
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductVO, Product>().ReverseMap();
                config.CreateMap<CartHeaderVO, CartHeader>().ReverseMap();
                config.CreateMap<CartDetailVO, CartDetail>().ReverseMap();
                config.CreateMap<Cart, CartVO>().ReverseMap().ReverseMap();
            });


            return mappingConfig;
        }
    }
}
