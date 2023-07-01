using ErrorOr;
using MediatR;
using RestaurantSimulation.Application.Authentication.Common;
using RestaurantSimulation.Application.Common.Interfaces.Persistence;
using RestaurantSimulation.Domain.Entities.Authentication;

namespace RestaurantSimulation.Application.Authentication.Queries.GetUsers
{
    public class GetUsersHandler : IRequestHandler<GetUsersQuery, ErrorOr<List<AuthenticationResult>>>
    {
        private readonly IUserRepository _userRepository;

        public GetUsersHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async  Task<ErrorOr<List<AuthenticationResult>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            List<User> users = await _userRepository.GetUsersAsync();

            return users.Select(user => new AuthenticationResult(user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                user.PhoneNumber,
                user.Address)).ToList();
        }
    }
}
