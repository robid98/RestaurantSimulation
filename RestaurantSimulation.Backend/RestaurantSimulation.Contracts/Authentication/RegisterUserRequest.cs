namespace RestaurantSimulation.Contracts.Authentication
{
    public record RegisterUserRequest(
        string FirstName,
        string LastName,
        string PhoneNumber,
        string Address);
}
