using FluentValidation;
using Para.Data.Domain;
using Para.Schema;

namespace Para.Business.Validations;

public class CustomerDetailValidator : AbstractValidator<CustomerDetailRequest>
{
    public CustomerDetailValidator()
    {
        RuleFor(x => x.FatherName)
            .NotEmpty()
            .MaximumLength(30)
            .WithMessage("FatherName cannot be empty.");

        RuleFor(x => x.MotherName)
            .NotEmpty()
            .MaximumLength(30)
            .WithMessage("MotherName cannot be empty.");

        RuleFor(x => x.EducationStatus)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("EducationStatus cannot be empty.");
        
        RuleFor(x => x.MontlyIncome)
            .NotEmpty()
            .MaximumLength(10)
            .WithMessage("MonthlyIncome cannot be empty.");

        RuleFor(x => x.Occupation)
            .NotEmpty()
            .MaximumLength(30)
            .WithMessage("Occupation cannot be empty.");
    }
}