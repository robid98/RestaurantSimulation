﻿namespace RestaurantSimulation.Contracts.Authentication
{
    public record AuthenticationResponse(
        Guid Id,
        string Email,
        string FirstName,
        string LastName,
        string PhoneNumber,
        string Address);
}
