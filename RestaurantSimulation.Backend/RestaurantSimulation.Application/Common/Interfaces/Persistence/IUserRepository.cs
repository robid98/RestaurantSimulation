using RestaurantSimulation.Domain.Entities.Authentication;

namespace RestaurantSimulation.Application.Common.Interfaces.Persistence
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);

        Task<User?> GetUserBySubAsync(string sub);

        Task AddAsync(User user);
    }
}
