using FluentValidation;
using SELLit.Common;

namespace SELLit.Server.Features.Products.Commands.Update;

public sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .NotNull()
            .WithMessage(ValidationConstants.ValidationMessage);
        
        RuleFor(x => x.Description)
            .NotEmpty()
            .NotNull()
            .WithMessage(ValidationConstants.ValidationMessage);
        
        RuleFor(x => x.Location)
            .NotEmpty()
            .NotNull()
            .WithMessage(ValidationConstants.ValidationMessage);
        
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .NotNull()
            .WithMessage(ValidationConstants.ValidationMessage);
        
        RuleFor(x => x.Price)
            .NotEmpty()
            .NotNull()
            .WithMessage(ValidationConstants.ValidationMessage);
        
        RuleFor(x => x.DeliveryResponsibility)
            .NotEmpty()
            .NotNull()
            .IsInEnum()
            .WithMessage(ValidationConstants.ValidationMessage);
    }
}