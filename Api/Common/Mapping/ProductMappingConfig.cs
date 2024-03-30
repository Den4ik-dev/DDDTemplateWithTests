using Application.Products.Commands.ChangeProduct;
using Contracts.Product;
using Mapster;

namespace Api.Common.Mapping;

public class ProductMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<(Guid ProductId, ChangeProductDto ChangeProductDto), ChangeProductCommand>()
            .Map(dest => dest.ProductId, src => src.ProductId)
            .Map(dest => dest.Name, src => src.ChangeProductDto.Name)
            .Map(dest => dest.Description, src => src.ChangeProductDto.Description)
            .Map(dest => dest.Price, src => src.ChangeProductDto.Price)
            .Map(dest => dest.CategoryId, src => src.ChangeProductDto.CategoryId);
    }
}
