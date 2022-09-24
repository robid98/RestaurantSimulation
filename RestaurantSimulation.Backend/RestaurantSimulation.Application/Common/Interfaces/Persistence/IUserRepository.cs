using ErrorOr;
using RestaurantSimulation.Domain.Entities.Authentication;

namespace RestaurantSimulation.Application.Common.Interfaces.Persistence
{
    public interface IUserRepository
    {
        Task AddAsync(User user);

        Task<ErrorOr<User>> UpdateAsync(User user);

        Task<User?> GetUserByEmailAsync(string email);

        Task<User?> GetUserBySubAsync(string sub);

        Task<User?> GetUserByIdAsync(Guid id);

        Task<List<User>> GetUsersAsync();
    }
}
