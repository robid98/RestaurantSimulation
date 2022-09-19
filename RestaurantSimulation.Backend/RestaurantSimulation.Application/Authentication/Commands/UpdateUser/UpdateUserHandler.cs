using ErrorOr;
using MediatR;
using RestaurantSimulation.Application.Authentication.Common;
using RestaurantSimulation.Application.Authentication.Common.Services.ExtractUserClaims;
using RestaurantSimulation.Application.Common.Interfaces.Persistence;
using RestaurantSimulation.Domain.Entities.Authentication;

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

            ErrorOr<User> user = await _userRepository.UpdateAsync(new User
            {
                Email = userEmail.Value,
                Sub = userSub.Value,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address
            });

            if (user.IsError)
                return user.FirstError;

            return new AuthenticationResult
            (
                user.Value.Id,
                user.Value.Email,
                user.Value.FirstName,
                user.Value.LastName,
                user.Value.PhoneNumber,
                user.Value.Address
            );
        }
    }
}
