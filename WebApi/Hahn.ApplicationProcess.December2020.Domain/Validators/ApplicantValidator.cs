using FluentValidation;
using Hahn.ApplicationProcess.December2020.Domain.Models;
using System.Linq;

namespace Hahn.ApplicationProcess.December2020.Domain.Validators
{
    public class ApplicantValidator : AbstractValidator<Applicant>
    {
        public ApplicantValidator()
        {
            RuleFor(a => a.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MinimumLength(5)
                .Must(BeAValidName).WithMessage("Name can only contain letters");

            RuleFor(f => f.FamilyName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MinimumLength(5)
                .Must(BeAValidName).WithMessage("FamilyName can only contain letters");

            RuleFor(a => a.Address)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MinimumLength(10);

            RuleFor(e => e.EmailAddress)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .EmailAddress().WithMessage("Email address is not valid")
            .Matches(@"^(([^<>()\[\]\\.,;:\s@""]+(\.[^<>()\[\]\\.,;:\s@""]+)*)|("".+""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,3}))$")
            .WithMessage("Email address is not properly formatted");

            RuleFor(a => a.Age)
                .ExclusiveBetween(20, 60);

            RuleFor(h => h.Hired)
                .NotNull();
        }

        private static bool BeAValidName(string name)
        {
            name = name.Replace(" ", "");
            name = name.Replace("-", "");

            return name.All(char.IsLetter);

        }

    }
}
