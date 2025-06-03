using AutoMapper;
using VDVT.Micro.Product.Api.Models.Dto;

namespace VDVT.Micro.Product.Api
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDto, Models.Product>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
