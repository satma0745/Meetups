namespace Meetups.Application.Features.Studio.DeleteSpecificMeetup.Api;

using System;
using System.Threading.Tasks;
using Meetups.Application.Features.Shared.Contracts.Auth;
using Meetups.Application.Features.Shared.Infrastructure.Api;
using Meetups.Application.Features.Studio.DeleteSpecificMeetup.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[Tags("Studio")]
public class Controller : ApiControllerBase
{
    private readonly RequestHandler requestHandler;

    public Controller(RequestHandler requestHandler) =>
        this.requestHandler = requestHandler;

    /// <summary>Deletes specific meetup (with the specified id).</summary>
    /// <param name="meetupId">Id of the meetup to be deleted.</param>
    /// <response code="200">Meetup was deleted successfully.</response>
    /// <response code="404">Meetup with the specified id does not exist.</response>
    [Authorize(Roles = UserRoles.Organizer)]
    [HttpDelete("studio/{meetupId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSpecificMeetup([FromRoute] Guid meetupId)
    {
        var internalRequest = new Request(meetupId, CurrentUser.UserId);
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