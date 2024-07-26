using FluentValidation;
using Para.Data.Domain;
using Para.Schema;

namespace Para.Business.Validations;

public class CustomerPhoneValidator : AbstractValidator<CustomerPhoneRequest>
{
    public CustomerPhoneValidator()
    {
        RuleFor(x => x.CountyCode)
            .NotEmpty()
            .MinimumLength(3)
            .WithMessage("CountryCode must be minimum 3 characters.");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .Length(10)
            .WithMessage("PhoneNumber must be 10 characters.");
    }
}