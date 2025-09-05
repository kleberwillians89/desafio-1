using MediatR;
using Hypesoft.Application.DTOs;

namespace Hypesoft.Application.Categories
{
    public sealed record ListCategoriesQuery(int Page, int PageSize, string? Name)
        : IRequest<PagedResult<CategoryResponse>>;

    public sealed record GetCategoryByIdQuery(string Id)
        : IRequest<CategoryResponse?>;
}
