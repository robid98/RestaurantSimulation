namespace RestaurantSimulation.Contracts.Authentication
{
    public record RegisterRequest(
        string FirstName,
        string LastName,
        string PhoneNumber,
        string Address);
}
