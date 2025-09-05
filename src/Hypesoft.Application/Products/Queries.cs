using Hypesoft.Application.DTOs;
using MediatR;

namespace Hypesoft.Application.Products;

// LIST
public sealed record ListProductsQuery(int Page, int PageSize, string? Name, string? CategoryId)
    : IRequest<PagedResult<ProductResponse>>;

// GET BY ID
public sealed record GetProductByIdQuery(string Id) : IRequest<ProductResponse>;

// DASHBOARD
public sealed record GetProductsDashboardQuery() : IRequest<ProductsDashboardResult>;
