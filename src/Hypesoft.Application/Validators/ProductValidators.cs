using FluentValidation;
using Hypesoft.Application.DTOs;

namespace Hypesoft.Application.Validators;

public sealed class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(2);
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.StockQty).GreaterThanOrEqualTo(0);
    }
}

public sealed class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("'Id' deve ser informado.");
        RuleFor(x => x.Name).NotEmpty().MinimumLength(2);
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.StockQty).GreaterThanOrEqualTo(0);
    }
}
