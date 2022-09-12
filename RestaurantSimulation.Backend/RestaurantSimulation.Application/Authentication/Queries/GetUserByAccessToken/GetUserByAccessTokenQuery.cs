using ErrorOr;
using MediatR;
using RestaurantSimulation.Application.Authentication.Common;

namespace RestaurantSimulation.Application.Authentication.Queries.GetUserByAccessToken
{
    public record GetUserByAccessTokenQuery() : IRequest<ErrorOr<AuthenticationResult>>;
}
