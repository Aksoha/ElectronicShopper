using System.Text.Json;
using ElectronicShopper.Library.Models.Internal;
using ElectronicShopper.Library.StoredProcedures.Category;
using ElectronicShopper.Library.StoredProcedures.Inventory;
using ElectronicShopper.Library.StoredProcedures.Order;
using ElectronicShopper.Library.StoredProcedures.Product;

namespace ElectronicShopper.Library.DependencyInjection;

public class DatabaseMappingProfile : Profile
{
    public DatabaseMappingProfile()
    {
        MapCategoryModels();
        MapProductModels();
        MapInventoryModels();
        MapOrderModels();
    }

    private void MapCategoryModels()
    {
        CreateMap<CategoryModel, CategoryDbModel>()
            .ForMember(dest => dest.CategoryName,
                act => act
                    .MapFrom(x => x.Name))
            .ForMember(dest => dest.ParentId,
                act => act
                    .MapFrom(x => x.Parent == null ? null : x.Parent.Id));

        CreateMap<CategoryDbModel, CategoryModel>()
            .ForMember(dest => dest.Name,
                act => act
                    .MapFrom(x => x.CategoryName));

        CreateMap<CategoryCreateModel, CategoryInsertStoredProcedure>()
            .ForMember(dest => dest.CategoryName,
                act => act
                    .MapFrom(x => x.Name));

        CreateMap<CategoryModel, CategoryGetAncestorStoredProcedure>();
        CreateMap<int, CategoryGetByIdStoredProcedure>()
            .ConvertUsing(x => new CategoryGetByIdStoredProcedure { Id = x });
    }
    

    private void MapProductModels()
    {
        CreateMap<ProductModel, ProductDbModel>()
            .ForMember(dest => dest.CategoryId,
                act => act
                    .MapFrom(x => x.Category.Id))
            .ForMember(dest => dest.Properties,
                act => act
                    .MapFrom((x, _) => JsonSerializer.Serialize(x.Properties)));

        CreateMap<ProductDbModel, ProductModel>()
            .ForMember(dest => dest.Properties,
                act => act
                    .MapFrom((x, _) =>
                        x.Properties == null
                            ? null
                            : JsonSerializer.Deserialize<Dictionary<string, List<string>>>(x.Properties)))
            .ForMember(dest => dest.Category,
                act => act
                    .MapFrom(x => new CategoryModel { Id = x.CategoryId }));

        CreateMap<ProductTemplateModel, ProductTemplateDbModel>()
            .ForMember(dest => dest.Properties,
                act => act
                    .MapFrom((x, _) => JsonSerializer.Serialize(x.Properties)));

        CreateMap<ProductTemplateDbModel, ProductTemplateModel>()
            .ForMember(dest => dest.Properties,
                act => act
                    .MapFrom((x, _) =>
                        x.Properties == null ? default : JsonSerializer.Deserialize<List<string>>(x.Properties)));

        CreateMap<ProductImageModel, ProductImageDbModel>().ReverseMap();

        CreateMap<ProductModel, ProductGetProductImagesStoredProcedure>()
            .ForMember(dest => dest.ProductId,
                act => act
                    .MapFrom(x => x.Id));
        
        CreateMap<ProductModel, ProductInsertStoredProcedure>()
            .ForMember(dest => dest.CategoryId,
                act => act
                    .MapFrom(x => x.Category.Id))
            .ForMember(dest => dest.Properties,
                act => act
                    .MapFrom((x, _) => x.Properties.Count == 0 ? null : JsonSerializer.Serialize(x.Properties)));

        CreateMap<ProductTemplateModel, ProductInsertStoredProcedure>()
            .ForMember(dest => dest.ProductTemplateId,
                act => act
                    .MapFrom(x => x.Id))
            .ForMember(dest => dest.Properties, 
                act  => act
                    .Ignore());

        CreateMap<ProductTemplateModel, ProductTemplateInsertStoredProcedure>()
            .ForMember(dest => dest.Properties,
                act => act
                    .MapFrom((x, _) => JsonSerializer.Serialize(x.Properties)));

        
        CreateMap<ProductModel, ProductImageInsertStoredProcedure>()
            .ForMember(dest => dest.ProductId,
                act => act
                    .MapFrom(x => x.Id));

        CreateMap<ProductImageModel, ProductImageInsertStoredProcedure>();



        CreateMap<ProductInsertModel, ProductInsertStoredProcedure>()
            .ForMember(dest => dest.ProductTemplateId,
                act => act
                    .MapFrom(x => x.Template == null ? null : x.Template.Id))
            .ForMember(dest => dest.Properties,
                act => act
                    .MapFrom((x, _) => x.Properties.Count == 0 ? null : JsonSerializer.Serialize(x.Properties)));

        CreateMap<MemoryImageModel, ProductImageInsertStoredProcedure>();
        
        CreateMap<MemoryImageModel, ProductImageModel>();

    }

    private void MapInventoryModels()
    {
        CreateMap<InventoryModel, InventoryDbModel>().ReverseMap()
            .ForMember(dest => dest.Quantity,
                act => act.MapFrom(x => x.QuantityAvailable))
            .ForMember(dest => dest.Reserved,
                act => act
                    .MapFrom(x => x.QuantityReserved));



        CreateMap<ProductModel, InventoryGetByProductIdStoredProcedure>()
            .ForMember(dest => dest.ProductId,
                act => act
                    .MapFrom(x => x.Id));
        

        CreateMap<InventoryModel, InventoryInsertStoredProcedure>()
            .ForMember(dest => dest.QuantityAvailable,
                act => act
                    .MapFrom(x => x.Quantity))
            .ForMember(dest => dest.QuantityReserved,
                act => act
                    .MapFrom(x => x.Reserved));

        CreateMap<InventoryModel, InventoryUpdateStoredProcedure>()
            .ForMember(dest => dest.QuantityAvailable,
                act => act
                    .MapFrom(x => x.Quantity))
            .ForMember(dest => dest.QuantityReserved,
                act => act
                    .MapFrom(x => x.Reserved));
        CreateMap<ProductModel, InventoryUpdateStoredProcedure>()
            .ForMember(dest => dest.ProductId,
                act => act
                    .MapFrom(x => x.Id));

    }

    private void MapOrderModels()
    {
        CreateMap<OrderModel, OrderDbModel>().ReverseMap();
        CreateMap<OrderModel, OrderInsertStoredProcedure>();
        CreateMap<OrderDetailModel, OrderDetailsInsertStoredProcedure>();
    }
}