using ErrorOr;
using MediatR;
using RestaurantSimulation.Application.Authentication.Common;

namespace RestaurantSimulation.Application.Authentication.Commands.UpdateUser
{
    public record UpdateUserCommand(
        string FirstName,
        string LastName,
        string PhoneNumber,
        string Address) : IRequest<ErrorOr<AuthenticationResult>>;
}
