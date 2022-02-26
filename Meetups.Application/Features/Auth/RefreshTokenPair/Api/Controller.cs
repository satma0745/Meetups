﻿namespace Meetups.Application.Features.Auth.RefreshTokenPair.Api;

using System.Threading.Tasks;
using Meetups.Application.Features.Auth.RefreshTokenPair.Internal;
using Meetups.Application.Features.Shared.Infrastructure.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[Tags("Auth")]
public class Controller : ApiControllerBase
{
    private readonly RequestHandler requestHandler;

    public Controller(RequestHandler requestHandler) =>
        this.requestHandler = requestHandler;

    /// <summary>Re-issue token pair using refresh token.</summary>
    /// <param name="oldRefreshToken">Refresh token.</param>
    /// <response code="200">Token pair was successfully re-issued.</response>
    /// <response code="400">Fake, damaged, expired or used refresh token was provided.</response>
    [HttpPost("auth/refresh")]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RefreshTokenPair([FromBody] string oldRefreshToken)
    {
        var internalRequest = new Request(oldRefreshToken);
        var internalResponse = await requestHandler.HandleRequest(internalRequest);
        return (internalResponse.Success, internalResponse.Payload, internalResponse.ErrorType) switch
        {
            (true, var internalResult, _) => Ok(internalResult.ToApiResponse()),
            (false, _, ErrorTypes.InvalidRefreshTokenProvided) => BadRequest(),
            (false, _, ErrorTypes.TokenBearerDoesNotExist) => BadRequest(),
            (false, _, ErrorTypes.FakeOrUsedRefreshTokenProvided) => BadRequest(),
            _ => InternalServerError()
        };
    }
}