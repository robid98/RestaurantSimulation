using Moq;
using RestaurantSimulation.Application.Common.Interfaces.Persistence;
using RestaurantSimulation.Domain.Entities.Authentication;

namespace RestaurantSimulation.UnitTests.Mocks.Authentication
{
    public static class MockUserRepository
    {
        public static Guid _guid1 = Guid.NewGuid();
        public static Guid _guid2 = Guid.NewGuid();

        public static List<User> userList = new List<User> {
                new User (
                    _guid1,
                    "auth0|sub1",
                    "robert98@yahoo.com",
                    "Robert",
                    "Darabana",
                    "0773111222",
                    "Piatra Neamt"
                ),
                new User (
                    _guid2,
                    "auth0|sub2",
                    "valibujor@yahoo.com",
                    "Vali",
                    "Bujor",
                    "0773111333",
                    "Piatra Neamt"
                ),
           };

        public static Mock<IUserRepository> GetUserRepository(List<User> users = null!)
        {
            var mockRepo = new Mock<IUserRepository>();

            if (users is null)
                users = userList;

            mockRepo.Setup(r => r.GetUsersAsync()).ReturnsAsync(users);

            mockRepo.Setup(r => r.AddAsync(It.IsAny<User>())).Returns((User user) =>
            {
                users.Add(user);
                return Task.FromResult(user);
            });

            mockRepo.Setup(r => r.GetUserByIdAsync(It.IsAny<Guid>())).Returns((Guid id) =>
            {
                return Task.FromResult(userList.FirstOrDefault(user => user.Id == id));
            });

            User? user = null;

            mockRepo.Setup(r => r.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);

            return mockRepo;
        }
    }
}
