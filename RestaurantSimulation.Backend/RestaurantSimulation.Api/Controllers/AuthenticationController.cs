using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantSimulation.Application.Authentication.Commands.Register;
using RestaurantSimulation.Application.Authentication.Common.Services.ExtractUserClaims;
using RestaurantSimulation.Contracts.Authentication;

namespace RestaurantSimulation.Api.Controllers
{
    [Route("api/auth")]
    [Authorize]
    public class AuthenticationController : ApiController
    {
        private readonly ISender _sender;
        private readonly IExtractUserClaimsService _extractUserClaimsService;

        public AuthenticationController(
            ISender mediator,
            IExtractUserClaimsService extractUserClaimsService)
        {
            _sender = mediator;
            _extractUserClaimsService = extractUserClaimsService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            ErrorOr<string> userEmail = _extractUserClaimsService.GetUserEmail();

            if (userEmail.IsError)
                return Problem(userEmail.Errors);

            ErrorOr<string> userSub = _extractUserClaimsService.GetUserSub();

            if (userSub.IsError)
                return Problem(userSub.Errors);

            var registerCommand = await _sender.Send(
                new RegisterCommand(
                    userSub.Value,
                    userEmail.Value,
                    request.FirstName,
                    request.LastName,
                    request.PhoneNumber,
                    request.Address)
                );

            return registerCommand.Match(
                registerResult => Ok(registerResult),
                errors => Problem(errors));
        }
    }
}
