using ErrorOr;
using MediatR;
using RestaurantSimulation.Application.Authentication.Common;
using RestaurantSimulation.Application.Authentication.Common.Services.ExtractUserClaims;
using RestaurantSimulation.Application.Common.Interfaces.Persistence;
using RestaurantSimulation.Domain.RestaurantApplicationErrors;
using RestaurantSimulation.Domain.Entities.Authentication;

namespace RestaurantSimulation.Application.Authentication.Queries.GetUserByAccessToken
{
    public class GetUserByAccessTokenHandler : IRequestHandler<GetUserByAccessTokenQuery, ErrorOr<AuthenticationResult>>
    {
        private readonly IExtractUserClaimsService _extractUserClaimsService;
        private readonly IUserRepository _userRepository;

        public GetUserByAccessTokenHandler(IExtractUserClaimsService extractUserClaimsService,
            IUserRepository userRepository)
        {
            _extractUserClaimsService = extractUserClaimsService;
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<AuthenticationResult>> Handle(GetUserByAccessTokenQuery request, CancellationToken cancellationToken)
        {
            ErrorOr<string> userSub = _extractUserClaimsService.GetUserSub();

            if (userSub.IsError)
                return userSub.Errors.FirstOrDefault();

            User? user = await _userRepository.GetUserBySubAsync(userSub.Value);

            if (user is null)
                return Errors.User.NotFound;

            return new AuthenticationResult(user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                user.PhoneNumber,
                user.Address);
        }
    }
}
