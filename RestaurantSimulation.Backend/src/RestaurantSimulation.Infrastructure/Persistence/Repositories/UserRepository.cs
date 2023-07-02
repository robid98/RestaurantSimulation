using Microsoft.EntityFrameworkCore;
using RestaurantSimulation.Application.Common.Interfaces.Persistence;
using RestaurantSimulation.Domain.Entities.Authentication;

namespace RestaurantSimulation.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly RestaurantSimulationContext _restaurantSimulationContext;

        public UserRepository(RestaurantSimulationContext restaurantSimulationContext)
        {
            _restaurantSimulationContext = restaurantSimulationContext;
        }

        public async Task AddAsync(User user)
        {
            await _restaurantSimulationContext.Users.AddAsync(user);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            User user = await _restaurantSimulationContext.Users.FirstOrDefaultAsync(user => user.Email == email);

            return user;
        }

        public async Task<User> GetUserBySubAsync(string sub)
        {
            User user = await _restaurantSimulationContext.Users.FirstOrDefaultAsync(user => user.Sub == sub);

            return user;
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            User user = await _restaurantSimulationContext.Users.FirstOrDefaultAsync(user => user.Id == id);

            return user;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            List<User> users = await _restaurantSimulationContext.Users.ToListAsync();

            return users;
        }
    }
}
