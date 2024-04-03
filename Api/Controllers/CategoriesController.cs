using Api.Common;
using Application.Categories.Commands.CreateCategory;
using Contracts.Category;
using Domain.Category.ValueObject;
using Domain.User.ValueObject;
using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/categories")]
public class CategoriesController : ApiController
{
    private readonly IMapper _mapper;
    private readonly ISender _sender;

    public CategoriesController(IMapper mapper, ISender sender)
    {
        _mapper = mapper;
        _sender = sender;
    }

    [HttpPost, Authorize(Roles = Roles.Admin)]
    public async Task<IResult> CreateCategory(CreateCategoryDto createCategoryDto)
    {
        CreateCategoryCommand command = _mapper.Map<CreateCategoryCommand>(createCategoryDto);

        Result<CategoryId> result = await _sender.Send(command);

        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }

        CategoryId categoryId = result.Value;

        return Results.Ok(categoryId.Value);
    }
}
