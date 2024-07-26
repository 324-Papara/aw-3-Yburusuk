using FluentValidation;
using Para.Data.Domain;
using Para.Schema;

namespace Para.Business.Validations;

public class CustomerAddressValidator : AbstractValidator<CustomerAddressRequest>
{
    public CustomerAddressValidator()
    {
        RuleFor(x => x.Country)
            .NotEmpty()
            .Length(1, 25)
            .WithMessage("Country cannot be empty.");

        RuleFor(x => x.City)
            .NotEmpty()
            .Length(1, 190)
            .WithMessage("City cannot be empty.");

        RuleFor(x => x.AddressLine)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("AddressLine cannot be empty.");

        RuleFor(x => x.ZipCode)
            .NotEmpty()
            .Length(5)
            .WithMessage("ZipCode cannot be empty and must be 5 characters.");
    }
}