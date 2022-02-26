using AutoMapper;
using ShoppingStore.ProductAPI.Data.ValueObjects;
using ShoppingStore.ProductAPI.Model;

namespace ShoppingStore.ProductAPI.Config
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductVO, Product>().ReverseMap();
            });


            return mappingConfig;
        }
    }
}
