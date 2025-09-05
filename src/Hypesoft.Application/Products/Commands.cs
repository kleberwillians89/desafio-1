using Hypesoft.Application.DTOs;
using MediatR;

namespace Hypesoft.Application.Products;

// CREATE
public sealed record CreateProductCommand(CreateProductRequest Data) : IRequest<string>;

// UPDATE
public sealed record UpdateProductCommand(string Id, UpdateProductRequest Data) : IRequest<bool>;

// DELETE
public sealed record DeleteProductCommand(string Id) : IRequest<bool>;
