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

        public AuthenticationController(
            ISender mediator)
        {
            _sender = mediator;
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
        public async Task<IActionResult> RegisterUser(RegisterUserRequest request)
        {
            var registerUserCommand = await _sender.Send(
                new RegisterUserCommand(
                    request.FirstName,
                    request.LastName,
                    request.PhoneNumber,
                    request.Address)
                );

            return registerUserCommand.Match(
                registerResult => Ok(GetAuthenticationResponse(registerResult)),
                errors => Problem(errors));
        }

        /// <summary>
        /// Update existing User
        /// </summary>
        [Authorize(Policy = AuthorizationPolicies.ClientOrAdminRolePolicy)]
        [HttpPut("users")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticationResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUser(UpdateUserRequest request)
        {
            var updateUserCommand = await _sender.Send(
                new UpdateUserCommand(
                    request.FirstName,
                    request.LastName,
                    request.PhoneNumber,
                    request.Address)
                );

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
