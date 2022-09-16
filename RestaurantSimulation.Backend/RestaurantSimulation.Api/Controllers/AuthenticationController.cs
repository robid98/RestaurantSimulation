using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantSimulation.Application.Authentication.Commands.Register;
using RestaurantSimulation.Application.Authentication.Common;
using RestaurantSimulation.Application.Authentication.Common.Services.ExtractUserClaims;
using RestaurantSimulation.Application.Authentication.Queries.GetUserByAccessToken;
using RestaurantSimulation.Application.Authentication.Queries.GetUserById;
using RestaurantSimulation.Application.Authentication.Queries.GetUsers;
using RestaurantSimulation.Contracts.Authentication;
using RestaurantSimulation.Domain.Common.Policies.Authorization;
using Swashbuckle.Swagger.Annotations;

namespace RestaurantSimulation.Api.Controllers
{
    [Route("api/auth")]
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

        /// <summary>
        /// Register a new User
        /// </summary>
        [Authorize(Policy = AuthorizationPolicies.ClientOrAdminRolePolicy)]
        [HttpPost("users")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticationResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
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
                registerResult => Ok(GetAuthenticationResponse(registerResult)),
                errors => Problem(errors));
        }

        /// <summary>
        ///Get current user
        /// </summary>
        [Authorize(Policy = AuthorizationPolicies.ClientOrAdminRolePolicy)]
        [HttpGet("user")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticationResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserByAccessToken()
        {
            var getUserByAccessTokenQuery = await _sender.Send(new GetUserByAccessTokenQuery());

            return getUserByAccessTokenQuery.Match(
                getUserByAccessTokenResult => Ok(GetAuthenticationResponse(getUserByAccessTokenResult)),
                errors => Problem(errors));
        }

        /// <summary>
        ///Get user by Id (admin role)
        /// </summary>
        [Authorize(Policy = AuthorizationPolicies.AdminRolePolicy)]
        [HttpGet("user/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticationResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var getUserByIdQuery = await _sender.Send(new GetUserByIdQuery(id));

            return getUserByIdQuery.Match(
                getUserByIdResult => Ok(GetAuthenticationResponse(getUserByIdResult)),
                errors => Problem(errors));
        }

        /// <summary>
        ///Get a list with all Users (admin role)
        /// </summary>
        [Authorize(Policy = AuthorizationPolicies.AdminRolePolicy)]
        [HttpGet("users")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AuthenticationResponse>))]
        public async Task<IActionResult> GetUsers()
        {
            var getUsersQuery = await _sender.Send(new GetUsersQuery());

            return getUsersQuery.Match(
                getUsersResult => Ok(GetAuthenticationResponseList(getUsersResult)),
                errors => Problem(errors));
        }

        private static AuthenticationResponse GetAuthenticationResponse(AuthenticationResult authenticationResult)
        {
            return new AuthenticationResponse(
                            authenticationResult.Id,
                            authenticationResult.Email,
                            authenticationResult.FirstName,
                            authenticationResult.LastName,
                            authenticationResult.PhoneNumber,
                            authenticationResult.Address);
        }

        private static List<AuthenticationResponse> GetAuthenticationResponseList(List<AuthenticationResult> authenticationResultList)
        {
            return authenticationResultList.Select(authenticationResult => new AuthenticationResponse(
                            authenticationResult.Id,
                            authenticationResult.Email,
                            authenticationResult.FirstName,
                            authenticationResult.LastName,
                            authenticationResult.PhoneNumber,
                            authenticationResult.Address)).ToList();
        }
    }
}
