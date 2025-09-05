using AutoMapper;
using Hypesoft.Application.DTOs;
using Hypesoft.Domain.Entities;

namespace Hypesoft.Application.Mapping;

public sealed class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductResponse>();
        CreateMap<CreateProductRequest, Product>();
        CreateMap<UpdateProductRequest, Product>();
    }
}
