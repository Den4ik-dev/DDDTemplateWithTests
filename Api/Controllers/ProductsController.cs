using Api.Common;
using Application.Products.Commands.ChangeProduct;
using Application.Products.Commands.CreateProduct;
using Contracts.Product;
using Domain.Product.ValueObject;
using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/products")]
public class ProductsController : ApiController
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public ProductsController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IResult> CreateProduct(CreateProductDto dto)
    {
        CreateProductCommand command = _mapper.Map<CreateProductCommand>(dto);

        Result<ProductId> result = await _sender.Send(command);

        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }

        ProductId productId = result.Value;

        return Results.Ok(productId.Value);
    }

    [HttpPut("{productId:guid}")]
    public async Task<IResult> ChangeProduct(Guid productId, ChangeProductDto dto)
    {
        ChangeProductCommand command = _mapper.Map<ChangeProductCommand>((productId, dto));

        Result result = await _sender.Send(command);

        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }

        return Results.NoContent();
    }
}
