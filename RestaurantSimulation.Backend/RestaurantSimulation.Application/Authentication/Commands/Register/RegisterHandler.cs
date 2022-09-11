using ErrorOr;
using MediatR;
using RestaurantSimulation.Application.Authentication.Common;
using RestaurantSimulation.Application.Common.Interfaces.Persistence;
using RestaurantSimulation.Domain.Common.Errors;
using RestaurantSimulation.Domain.Entities.Authentication;

namespace RestaurantSimulation.Application.Authentication.Commands.Register
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
    {
        private readonly IUserRepository _userRepository;

        public RegisterHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {

            if (await _userRepository.GetUserByEmailAsync(request.Email) is not null)
            {
                return Errors.User.DuplicateEmail;
            }

            var userId = Guid.NewGuid();

            var user = new User
            {
                Id = userId,
                Sub = request.Sub,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address
            };

            await _userRepository.AddAsync(user);

            return new AuthenticationResult(
                userId,
                request.Email,
                request.FirstName,
                request.LastName,
                request.PhoneNumber,
                request.Address);
        }
    }
}
