using AutoMapper;
using Hypesoft.Application.DTOs;
using Hypesoft.Domain.Entities;

namespace Hypesoft.Application.Mapping
{
    public sealed class Profiles : Profile
    {
        public Profiles()
        {
            CreateMap<Product, ProductResponse>();
            CreateMap<CreateProductRequest, Product>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.UpdatedAt, o => o.Ignore());
            CreateMap<UpdateProductRequest, Product>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.UpdatedAt, o => o.Ignore());

            CreateMap<Category, CategoryResponse>();
        }
    }
}
