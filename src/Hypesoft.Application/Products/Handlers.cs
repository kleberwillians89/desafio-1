using System.Linq;
using AutoMapper;
using Hypesoft.Application.DTOs;
using Hypesoft.Domain.Entities;
using Hypesoft.Infrastructure.Repositories;
using MediatR;

namespace Hypesoft.Application.Products;

public sealed class ProductHandlers :
    IRequestHandler<CreateProductCommand, string>,
    IRequestHandler<UpdateProductCommand, bool>,
    IRequestHandler<DeleteProductCommand, bool>,
    IRequestHandler<ListProductsQuery, PagedResult<ProductResponse>>,
    IRequestHandler<GetProductByIdQuery, ProductResponse>,
    IRequestHandler<GetProductsDashboardQuery, ProductsDashboardResult>
{
    private readonly IProductRepository _products;
    private readonly ICategoryRepository _categories;
    private readonly IMapper _mapper;

    public ProductHandlers(IProductRepository products, ICategoryRepository categories, IMapper mapper)
    {
        _products   = products;
        _categories = categories;
        _mapper     = mapper;
    }

    // CREATE
    public async Task<string> Handle(CreateProductCommand request, CancellationToken ct)
    {
        var dto = request.Data;

        var cat = await _categories.GetByIdAsync(dto.CategoryId, ct);
        if (cat is null) throw new InvalidOperationException("Categoria não encontrada.");

        var entity = new Product
        {
            Id          = Guid.NewGuid().ToString("N"),
            Name        = dto.Name,
            Description = dto.Description,
            Price       = dto.Price,
            CategoryId  = dto.CategoryId,
            StockQty    = dto.StockQty,
            CreatedAt   = DateTime.UtcNow,
            UpdatedAt   = DateTime.UtcNow
        };

        await _products.CreateAsync(entity, ct);
        return entity.Id;
    }

    // UPDATE
    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken ct)
    {
        var dto = request.Data;

        var cat = await _categories.GetByIdAsync(dto.CategoryId, ct);
        if (cat is null) throw new InvalidOperationException("Categoria não encontrada.");

        var existing = await _products.GetByIdAsync(request.Id, ct);
        if (existing is null) return false;

        existing.Name        = dto.Name;
        existing.Description = dto.Description;
        existing.Price       = dto.Price;
        existing.CategoryId  = dto.CategoryId;
        existing.StockQty    = dto.StockQty;
        existing.UpdatedAt   = DateTime.UtcNow;

        await _products.UpdateAsync(existing, ct);
        return true;
    }

    // DELETE
    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken ct)
    {
        var existing = await _products.GetByIdAsync(request.Id, ct);
        if (existing is null) return false;

        await _products.DeleteAsync(request.Id, ct);
        return true;
    }

    // LIST
    public async Task<PagedResult<ProductResponse>> Handle(ListProductsQuery request, CancellationToken ct)
    {
        var (items, total) = await _products.GetPagedAsync(
            request.Page,
            request.PageSize,
            request.Name,
            request.CategoryId,
            ct
        );

        var mapped     = items.Select(_mapper.Map<ProductResponse>).ToList();
        var totalCount = (int)total; // repo retorna long -> converte

        return new PagedResult<ProductResponse>
        {
            Items    = mapped,
            Page     = request.Page,
            PageSize = request.PageSize,
            Total    = totalCount
            // TotalPages é calculado dentro de PagedResult
        };
    }

    // GET BY ID
    public async Task<ProductResponse> Handle(GetProductByIdQuery request, CancellationToken ct)
    {
        var entity = await _products.GetByIdAsync(request.Id, ct)
            ?? throw new KeyNotFoundException("Produto não encontrado.");

        return _mapper.Map<ProductResponse>(entity);
    }

    // DASHBOARD
    public async Task<ProductsDashboardResult> Handle(GetProductsDashboardQuery request, CancellationToken ct)
    {
        var totalProducts = (int)await _products.GetTotalCountAsync(ct);
        var totalValue    = await _products.GetTotalInventoryValueAsync(ct);
        var lowStock      = await _products.GetLowStockAsync(10, ct);
        var byCategory    = await _products.GetCountByCategoryAsync(ct);

        var lowStockDtos = lowStock.Select(_mapper.Map<ProductResponse>).ToList();

        return new ProductsDashboardResult(
            TotalProducts: totalProducts,
            TotalInventoryValue: totalValue,
            LowStockItems: lowStockDtos,
            CountByCategory: byCategory
        );
    }
}
