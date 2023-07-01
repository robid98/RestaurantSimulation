using FluentValidation;

namespace RestaurantSimulation.Application.Authentication.Commands.RegisterUser
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(25)
                .Matches("^[a-zA-Z\\- ]+$");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(25)
                .Matches("^[a-zA-Z\\- ]+$");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Length(10)
                .Matches("^[0-9]+$");

            RuleFor(x => x.Address)
                .NotEmpty()
                .Length(3, 100)
                .Matches("^[a-zA-Z0-9\\- ,.]+$");
        }
    }
}
