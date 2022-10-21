using System.Text.Json;
using AutoMapper;
using ElectronicShopper.DataAccess.Models.Internal;
using ElectronicShopper.Library.Models;

namespace ElectronicShopper.DataAccess.DependencyInjection;

public class DatabaseMappingProfile : Profile
{
    public DatabaseMappingProfile()
    {
        MapCategoryModels();
        MapUserModels();
        MapProductModels();
        MapInventoryModels();
        MapOrderModels();
    }

    private void MapCategoryModels()
    {
        CreateMap<CategoryModel, CategoryDbModel>()
            .ForMember(dest => dest.CategoryName,
                act
                    => act.MapFrom(x => x.Name))
            .ForMember(dest => dest.ParentId,
                act
                    => act.MapFrom(x => x.Ancestors == null ? default : x.Ancestors.Last().Id));

        CreateMap<CategoryDbModel, CategoryModel>()
            .ForMember(dest => dest.Name,
                act
                    => act.MapFrom(x => x.CategoryName));
    }

    private void MapUserModels()
    {
    
    }
    
    private void MapProductModels()
    {
        CreateMap<ProductModel, ProductDbModel>()
            .ForMember(dest => dest.CategoryId,
                act
                    => act.MapFrom(x => x.Category.Id))
            .ForMember(dest => dest.Properties,
                act
                    => act.MapFrom((x, _) => JsonSerializer.Serialize(x.Properties)));

        CreateMap<ProductDbModel, ProductModel>()
            .ForMember(dest => dest.Properties,
                act
                    => act.MapFrom((x, _) =>
                        x.Properties == null
                            ? default
                            : JsonSerializer.Deserialize<Dictionary<string, List<string>>>(x.Properties)))
            .ForMember(dest => dest.Category,
                act
                    => act.MapFrom(x => new CategoryModel { Id = x.CategoryId }));

        CreateMap<ProductTemplateModel, ProductTemplateDbModel>()
            .ForMember(dest => dest.Properties,
                act
                    => act.MapFrom((x, _) => JsonSerializer.Serialize(x.Properties)));

        CreateMap<ProductTemplateDbModel, ProductTemplateModel>()
            .ForMember(dest => dest.Properties,
                act
                    => act.MapFrom((x, _) =>
                        x.Properties == null ? default : JsonSerializer.Deserialize<List<string>>(x.Properties)));

        CreateMap<ProductImageModel, ProductImageDbModel>().ReverseMap();
    }

    private void MapInventoryModels()
    {
        CreateMap<InventoryModel, InventoryDbModel>().ReverseMap();
    }

    private void MapOrderModels()
    {
        CreateMap<OrderModel, OrderDbModel>().ReverseMap();
    }
}