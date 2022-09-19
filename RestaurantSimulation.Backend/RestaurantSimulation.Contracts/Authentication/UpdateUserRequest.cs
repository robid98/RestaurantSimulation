namespace RestaurantSimulation.Contracts.Authentication
{
    public record UpdateUserRequest(
        string FirstName,
        string LastName,
        string PhoneNumber,
        string Address);
}
