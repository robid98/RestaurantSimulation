using ErrorOr;
using MediatR;
using RestaurantSimulation.Application.Authentication.Common;
using RestaurantSimulation.Application.Authentication.Common.Services.ExtractUserClaims;
using RestaurantSimulation.Application.Common.Interfaces.Persistence;
using RestaurantSimulation.Domain.Entities.Authentication;
using RestaurantSimulation.Domain.RestaurantApplicationErrors;

namespace RestaurantSimulation.Application.Authentication.Commands.UpdateUser
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, ErrorOr<AuthenticationResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IExtractUserClaimsService _extractUserClaimsService;

        public UpdateUserHandler(IUserRepository userRepository,
            IExtractUserClaimsService extractUserClaimsService)
        {
            _userRepository = userRepository;
            _extractUserClaimsService = extractUserClaimsService;
        }

        public async Task<ErrorOr<AuthenticationResult>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            ErrorOr<string> userEmail = _extractUserClaimsService.GetUserEmail();

            if (userEmail.IsError)
                return userEmail.FirstError;

            ErrorOr<string> userSub = _extractUserClaimsService.GetUserSub();

            if (userSub.IsError)
                return userSub.FirstError;

            User? user = await _userRepository.GetUserByEmailAsync(userEmail.Value);

            if (user is null)
            {
                return Errors.User.NotFound;
            }

            User.UpdateUserProfile(user,
                request.FirstName,
                request.LastName,
                request.PhoneNumber,
                request.Address);

            await _userRepository.UpdateAsync();

            return new AuthenticationResult
            (
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                user.PhoneNumber,
                user.Address
            );
        }
    }
}
