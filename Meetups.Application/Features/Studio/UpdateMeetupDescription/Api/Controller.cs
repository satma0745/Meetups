namespace Meetups.Application.Features.Studio.UpdateMeetupDescription.Api;

using System;
using System.Threading.Tasks;
using Meetups.Application.Features.Shared.Contracts.Auth;
using Meetups.Application.Features.Shared.Infrastructure.Api;
using Meetups.Application.Features.Studio.UpdateMeetupDescription.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[Tags("Studio")]
public class Controller : ApiControllerBase
{
    private readonly RequestHandler requestHandler;

    public Controller(RequestHandler requestHandler) =>
        this.requestHandler = requestHandler;
    
    /// <summary>Updates description of a specific meetup (with the specified ID).</summary>
    /// <param name="meetupId">Meetup ID to update description of.</param>
    /// <param name="requestDto">DTO with the updated meetup description.</param>
    /// <response code="200">Meetup description was updated successfully.</response>
    /// <response code="403">Only the organizer of a meetup can update it's description.</response>
    /// <response code="404">Specified meetup does not exist.</response>
    [Authorize(Roles = UserRoles.Organizer)]
    [HttpPut("studio/{meetupId:guid}/description")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RescheduleMeetup([FromRoute] Guid meetupId, [FromBody] RequestDto requestDto)
    {
        var internalRequest = requestDto.ToInternalRequest(meetupId, CurrentUser);
        var internalResponse = await requestHandler.HandleRequest(internalRequest);
        return (internalResponse.Success, internalResponse.ErrorType) switch
        {
            (true, _) => Ok(),
            (false, ErrorTypes.MeetupDoesNotExist) => NotFound(),
            (false, ErrorTypes.AccessViolation) => Forbid(),
            _ => InternalServerError()
        };
    }
}