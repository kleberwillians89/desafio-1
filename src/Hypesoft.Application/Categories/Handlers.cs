using AutoMapper;
using Hypesoft.Application.DTOs;
using Hypesoft.Domain.Entities;
using Hypesoft.Infrastructure.Repositories;
using MediatR;

namespace Hypesoft.Application.Categories;

public sealed class ListCategoriesHandler : IRequestHandler<ListCategoriesQuery, PagedResult<CategoryResponse>>
{
    private readonly ICategoryRepository _repo;
    private readonly IMapper _mapper;

    public ListCategoriesHandler(ICategoryRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<PagedResult<CategoryResponse>> Handle(ListCategoriesQuery request, CancellationToken ct)
    {
        var (items, total) = await _repo.GetPagedAsync(request.Page, request.PageSize, request.Name, ct);
        var dtos = _mapper.Map<IReadOnlyList<CategoryResponse>>(items);

        return new PagedResult<CategoryResponse>
        {
            Page = request.Page,
            PageSize = request.PageSize,
            Total = (int)total,
            Items = dtos
        };
    }
}

public sealed class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, CategoryResponse?>
{
    private readonly ICategoryRepository _repo;
    private readonly IMapper _mapper;

    public GetCategoryByIdHandler(ICategoryRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<CategoryResponse?> Handle(GetCategoryByIdQuery request, CancellationToken ct)
    {
        var cat = await _repo.GetByIdAsync(request.Id, ct);
        return cat is null ? null : _mapper.Map<CategoryResponse>(cat);
    }
}

public sealed class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, string>
{
    private readonly ICategoryRepository _repo;
    private readonly IMapper _mapper;

    public CreateCategoryHandler(ICategoryRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<string> Handle(CreateCategoryCommand request, CancellationToken ct)
    {
        var entity = new Category
        {
            Id = Guid.NewGuid().ToString("N"),
            Name = request.Data.Name,
            Description = request.Data.Description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true
        };
        return await _repo.CreateAsync(entity, ct);
    }
}

public sealed class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, bool>
{
    private readonly ICategoryRepository _repo;

    public UpdateCategoryHandler(ICategoryRepository repo) => _repo = repo;

    public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken ct)
    {
        var existing = await _repo.GetByIdAsync(request.Id, ct);
        if (existing is null) return false;

        existing.Name = request.Data.Name;
        existing.Description = request.Data.Description;
        existing.UpdatedAt = DateTime.UtcNow;

        return await _repo.UpdateAsync(existing, ct);
    }
}

public sealed class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, bool>
{
    private readonly ICategoryRepository _repo;
    public DeleteCategoryHandler(ICategoryRepository repo) => _repo = repo;

    public Task<bool> Handle(DeleteCategoryCommand request, CancellationToken ct)
        => _repo.DeleteAsync(request.Id, ct);
}
