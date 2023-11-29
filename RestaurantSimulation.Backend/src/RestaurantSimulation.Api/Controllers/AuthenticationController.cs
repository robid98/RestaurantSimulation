using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantSimulation.Application.Authentication.Commands.RegisterUser;
using RestaurantSimulation.Application.Authentication.Commands.UpdateUser;
using RestaurantSimulation.Application.Authentication.Common;
using RestaurantSimulation.Application.Authentication.Queries.GetUserByAccessToken;
using RestaurantSimulation.Application.Authentication.Queries.GetUserById;
using RestaurantSimulation.Application.Authentication.Queries.GetUsers;
using RestaurantSimulation.Contracts.Authentication;
using RestaurantSimulation.Domain.Common.Policies.Authorization;

namespace RestaurantSimulation.Api.Controllers
{
    [Route("api/auth")]
    public class AuthenticationController : ApiController
    {
        private readonly ISender _sender;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(
            ISender mediator,
            ILogger<AuthenticationController> logger)
        {
            _sender = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Register a new User
        /// </summary>
        [Authorize(Policy = AuthorizationPolicies.ClientOrAdminRolePolicy)]
        [HttpPost("user")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticationResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RegisterUser(RegisterUserRequest request)
        {
            _logger.LogDebug($"Entering {nameof(RegisterUser)} in {nameof(AuthenticationController)}");

            ErrorOr<AuthenticationResult> registerUserCommand = await _sender.Send(
                new RegisterUserCommand(
                    request.FirstName,
                    request.LastName,
                    request.PhoneNumber,
                    request.Address)
                );

            _logger.LogDebug($"Exiting {nameof(RegisterUser)} in {nameof(AuthenticationController)}");

            return registerUserCommand.Match(
                registerResult => Ok(GetAuthenticationResponse(registerResult)),
                errors => Problem(errors));
        }

        /// <summary>
        /// Update existing User
        /// </summary>
        [Authorize(Policy = AuthorizationPolicies.ClientOrAdminRolePolicy)]
        [HttpPut("user")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticationResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateUser(UpdateUserRequest request)
        {
            _logger.LogDebug($"Entering {nameof(UpdateUser)} in {nameof(AuthenticationController)}");

            ErrorOr<AuthenticationResult> updateUserCommand = await _sender.Send(
                new UpdateUserCommand(
                    request.FirstName,
                    request.LastName,
                    request.PhoneNumber,
                    request.Address)
                );

            _logger.LogDebug($"Exiting {nameof(UpdateUser)} in {nameof(AuthenticationController)}");

            return updateUserCommand.Match(
                updateResult => Ok(GetAuthenticationResponse(updateResult)),
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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetUserByAccessToken()
        {
            _logger.LogDebug($"Entering {nameof(GetUserByAccessToken)} in {nameof(AuthenticationController)}");

            ErrorOr<AuthenticationResult> getUserByAccessTokenQuery = await _sender.Send(new GetUserByAccessTokenQuery());

            _logger.LogDebug($"Exiting {nameof(GetUserByAccessToken)} in {nameof(AuthenticationController)}");

            return getUserByAccessTokenQuery.Match(
                getUserByAccessTokenResult => Ok(GetAuthenticationResponse(getUserByAccessTokenResult)),
                errors => Problem(errors));
        }

        /// <summary>
        /// Get user by Id (admin role)
        /// </summary>
        [Authorize(Policy = AuthorizationPolicies.AdminRolePolicy)]
        [HttpGet("user/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticationResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            _logger.LogDebug($"Entering {nameof(GetUserById)} in {nameof(AuthenticationController)}");

            ErrorOr<AuthenticationResult> getUserByIdQuery = await _sender.Send(new GetUserByIdQuery(id));

            _logger.LogDebug($"Exiting {nameof(GetUserById)} in {nameof(AuthenticationController)}");

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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetUsers()
        {
            _logger.LogDebug($"Entering {nameof(GetUsers)} in {nameof(AuthenticationController)}");

            ErrorOr<List<AuthenticationResult>> getUsersQuery = await _sender.Send(new GetUsersQuery());

            _logger.LogDebug($"Exiting {nameof(GetUsers)} in {nameof(AuthenticationController)}");

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
