using AutoMapper;
using ElectronicShopper.Library.Models;

namespace ElectronicShopper.Library.DependencyInjection;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateProductModel, ProductModel>();
        CreateMap<MemoryImageModel, ProductImageModel>();
    }
}