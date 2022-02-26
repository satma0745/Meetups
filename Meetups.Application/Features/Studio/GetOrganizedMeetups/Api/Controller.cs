namespace Meetups.Application.Features.Studio.GetOrganizedMeetups.Api;

using System.Threading.Tasks;
using Meetups.Application.Features.Shared.Contracts.Auth;
using Meetups.Application.Features.Shared.Infrastructure.Api;
using Meetups.Application.Features.Studio.GetOrganizedMeetups.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[Tags("Studio")]
public class Controller : ApiControllerBase
{
    private readonly RequestHandler requestHandler;

    public Controller(RequestHandler requestHandler) =>
        this.requestHandler = requestHandler;

    /// <summary>Get a list of all meetups organized by the current user.</summary>
    /// <response code="200">Meetups organized by the current user.</response>
    [Authorize(Roles = UserRoles.Organizer)]
    [HttpGet("studio/organized")]
    [ProducesResponseType(typeof(ResponseDto[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrganizedMeetups()
    {
        var internalRequest = new Request(CurrentUser.UserId);
        var internalResponse = await requestHandler.HandleRequest(internalRequest);
        return (internalResponse.Success, internalResponse.Payload) switch
        {
            (true, var internalResult) => Ok(internalResult.ToApiResponse()),
            (false, _) => InternalServerError()
        };
    }
}