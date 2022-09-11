using ErrorOr;
using MediatR;
using RestaurantSimulation.Application.Authentication.Common;

namespace RestaurantSimulation.Application.Authentication.Commands.Register
{
    public record RegisterCommand(
        string Sub,
        string Email,
        string FirstName,
        string LastName,
        string PhoneNumber,
        string Address) : IRequest<ErrorOr<AuthenticationResult>>;
}
