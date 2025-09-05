namespace Hypesoft.Application.DTOs;

public sealed record ProductResponse(
    string Id,
    string Name,
    string? Description,
    decimal Price,
    string CategoryId,
    int StockQty
);

public sealed record CreateProductRequest(
    string Name,
    string? Description,
    decimal Price,
    string CategoryId,
    int StockQty
);

public sealed record UpdateProductRequest(
    string Id,
    string Name,
    string? Description,
    decimal Price,
    string CategoryId,
    int StockQty
);

// usado no /api/products/dashboard
public sealed record ProductsDashboardResult(
    int TotalProducts,
    decimal TotalInventoryValue,
    IReadOnlyList<ProductResponse> LowStockItems,
    IReadOnlyList<(string CategoryId, int Count)> CountByCategory
);
