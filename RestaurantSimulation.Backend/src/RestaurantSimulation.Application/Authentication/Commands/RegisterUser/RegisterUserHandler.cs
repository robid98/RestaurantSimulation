using Microsoft.Extensions.Logging;
using ErrorOr;
using MediatR;
using RestaurantSimulation.Application.Authentication.Common;
using RestaurantSimulation.Application.Authentication.Common.Services.ExtractUserClaims;
using RestaurantSimulation.Application.Common.Interfaces.Persistence;
using RestaurantSimulation.Domain.RestaurantApplicationErrors;
using RestaurantSimulation.Domain.Entities.Authentication;

namespace RestaurantSimulation.Application.Authentication.Commands.RegisterUser
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, ErrorOr<AuthenticationResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IExtractUserClaimsService _extractUserClaimsService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RegisterUserHandler> _logger;

        public RegisterUserHandler(
            IUserRepository userRepository,
            IExtractUserClaimsService extractUserClaimsService,
            IUnitOfWork unitOfWork,
            ILogger<RegisterUserHandler> logger)
        {
            _userRepository = userRepository;
            _extractUserClaimsService = extractUserClaimsService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            ErrorOr<string> userEmail = _extractUserClaimsService.GetUserEmail();

            if (userEmail.IsError)
            {
                return userEmail.FirstError;
            }

            ErrorOr<string> userSub = _extractUserClaimsService.GetUserSub();

            if (userSub.IsError)
            {
                return userSub.FirstError;
            }

            if (await _userRepository.GetUserByEmailAsync(userEmail.Value) is not null)
            {
                _logger.LogError("The email {@Email} already exists in RestaurantSimulation. Error returned {@RestaurantSimulationDomainError}", userEmail.Value, Errors.User.DuplicateEmail);

                return Errors.User.DuplicateEmail;
            }

            var userId = Guid.NewGuid();

            var user = new User(userId, userSub.Value, userEmail.Value, request.FirstName, request.LastName, request.PhoneNumber, request.Address);

            await _userRepository.AddAsync(user);

            await _unitOfWork.SaveChangesAsync();

            return new AuthenticationResult(
                userId,
                userEmail.Value,
                request.FirstName,
                request.LastName,
                request.PhoneNumber,
                request.Address);
        }
    }
}
