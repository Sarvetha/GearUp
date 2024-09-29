using AutoMapper;
using GearUp.Models;
using GearUp.Models.ResponseModels;
namespace GearUp.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductResponseModel>().ReverseMap();
        }
    }
}
