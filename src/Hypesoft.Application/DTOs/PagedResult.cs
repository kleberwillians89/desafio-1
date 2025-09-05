using System;
using System.Collections.Generic;

namespace Hypesoft.Application.DTOs;

public sealed class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int Total { get; init; }

    public int TotalPages => PageSize <= 0 ? 0 : (int)Math.Ceiling((double)Total / PageSize);
}