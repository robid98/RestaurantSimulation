using Moq;
using RestaurantSimulation.Application.Common.Interfaces.Persistence;
using RestaurantSimulation.Domain.Entities.Authentication;

namespace RestaurantSimulation.Tests.UnitTests.Mocks.Authentication
{
    public static class MockUserRepository
    {
        public static Guid _guid1 = Guid.NewGuid();
        public static Guid _guid2 = Guid.NewGuid();

        public static List<User> userList = new List<User> {
                new User {
                    Id = _guid1,
                    Email = "robert98@yahoo.com",
                    FirstName = "Robert",
                    LastName = "Darabana",
                    Address = "Piatra Neamt",
                    PhoneNumber = "0773111222"
                },
                new User {
                    Id = _guid2,
                    Email = "valibujor@yahoo.com",
                    FirstName = "Vali",
                    LastName = "Bujor",
                    Address = "Piatra Neamt",
                    PhoneNumber = "0773111333"
                },
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
