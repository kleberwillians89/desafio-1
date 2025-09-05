using FluentValidation;
using Hypesoft.Application.DTOs;

namespace Hypesoft.Application.Validators;

public sealed class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("'Name' é obrigatório.")
            .MaximumLength(120).WithMessage("'Name' deve ter no máximo 120 caracteres.");
    }
}

public sealed class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
{
    public UpdateCategoryRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("'Id' deve ser informado.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("'Name' é obrigatório.")
            .MaximumLength(120).WithMessage("'Name' deve ter no máximo 120 caracteres.");
    }
}
