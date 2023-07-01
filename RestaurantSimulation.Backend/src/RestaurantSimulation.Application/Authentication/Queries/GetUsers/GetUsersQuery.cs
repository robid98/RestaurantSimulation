using ErrorOr;
using MediatR;
using RestaurantSimulation.Application.Authentication.Common;

namespace RestaurantSimulation.Application.Authentication.Queries.GetUsers
{
    public record GetUsersQuery() : IRequest<ErrorOr<List<AuthenticationResult>>>;
}
