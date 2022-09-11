namespace RestaurantSimulation.Application.Authentication.Common
{
    public record AuthenticationResult(
        Guid Id,
        string Email,
        string FirstName,
        string LastName,
        string PhoneNumber,
        string Address
    );
}
