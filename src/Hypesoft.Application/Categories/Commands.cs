using Hypesoft.Application.DTOs;
using MediatR;

namespace Hypesoft.Application.Categories
{
    public sealed record CreateCategoryCommand(CreateCategoryRequest Data) : IRequest<string>;
    public sealed record UpdateCategoryCommand(string Id, UpdateCategoryRequest Data) : IRequest<bool>;
    public sealed record DeleteCategoryCommand(string Id) : IRequest<bool>;
}
