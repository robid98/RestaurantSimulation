using ErrorOr;
using MediatR;
using RestaurantSimulation.Application.Authentication.Common;

namespace RestaurantSimulation.Application.Authentication.Commands.RegisterUser
{
    public record RegisterUserCommand(
        string FirstName,
        string LastName,
        string PhoneNumber,
        string Address) : IRequest<ErrorOr<AuthenticationResult>>;
}
