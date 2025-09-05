using AutoMapper;
using Hypesoft.Application.DTOs;
using Hypesoft.Domain.Entities;

namespace Hypesoft.Application.Mapping;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryResponse>();
    }
}
