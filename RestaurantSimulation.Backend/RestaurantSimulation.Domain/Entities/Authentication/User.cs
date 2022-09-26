using RestaurantSimulation.Domain.Primitives;

namespace RestaurantSimulation.Domain.Entities.Authentication
{
    public class User : Entity
    {
        public User(Guid id, string sub, string email, string firstName, string lastName, string phoneNumber, string address) : base(id)
        {
            Sub = sub;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Address = address;
        }

        public string Sub { get; private set; } = default!;

        public string Email { get; private set; } = default!;

        public string FirstName { get; private set; } = default!;

        public string LastName { get; private set; } = default!;

        public string PhoneNumber { get; private set; } = default!;

        public string Address { get; private set; } = default!;

        public static void UpdateUserProfile(
            User user, 
            string firstName,
            string lastName,
            string PhoneNumber,
            string Address)
        {
            user.FirstName = firstName;
            user.LastName = lastName;
            user.PhoneNumber = PhoneNumber;
            user.Address = Address;
        }
    }
}
