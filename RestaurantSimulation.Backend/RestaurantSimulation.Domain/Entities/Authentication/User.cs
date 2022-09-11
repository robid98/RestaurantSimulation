namespace RestaurantSimulation.Domain.Entities.Authentication
{
    public class User
    {
        public Guid Id { get; set; }

        public string Sub { get; set; } = default!;

        public string Email { get; set; } = default!;

        public string FirstName { get; set; } = default!;

        public string LastName { get; set; } = default!;

        public string PhoneNumber { get; set; } = default!;

        public string Address { get; set; } = default!;
    }
}
