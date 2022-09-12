using ErrorOr;
using MediatR;
using RestaurantSimulation.Application.Authentication.Common;

namespace RestaurantSimulation.Application.Authentication.Queries.GetUserById
{
    public record GetUserByIdQuery(Guid Id) : IRequest<ErrorOr<AuthenticationResult>>;
}
