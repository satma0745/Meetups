namespace Meetups.Application.Features.Feed.GetSignedUpGuestsInfo.Api;

using System;
using System.Linq;
using System.Threading.Tasks;
using Meetups.Application.Features.Feed.GetSignedUpGuestsInfo.Internal;
using Meetups.Application.Seedwork.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[Tags("Feed")]
public class Controller : ApiControllerBase
{
    private readonly RequestHandler requestHandler;

    public Controller(RequestHandler requestHandler) =>
        this.requestHandler = requestHandler;

    /// <summary>Get information about all guest who signed up for the meetup.</summary>
    /// <param name="meetupId">Meetup ID.</param>
    /// <response code="200">Guest who signed up for the meetup.</response>
    /// <response code="404">Meetup with the specified ID does not exist.</response>
    [HttpGet("feed/{meetupId:guid}/signed-up-guests")]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IActionResult> GetSignedUpGuestsInfo([FromRoute] Guid meetupId) =>
        ApiPipeline
            .CreateRequest(new Request(meetupId))
            .HandleRequestAsync(requestHandler)
            .ToApiResponse(
                onSuccess: result =>
                {
                    var guests = result.Select(guest => new GuestDto(
                        id: guest.Id,
                        displayName: guest.DisplayName));
                    return Ok(new ResponseDto(guests));
                },
                onFailure: errorTypes => errorTypes switch
                {
                    ErrorTypes.MeetupDoesNotExist => NotFound(),
                    _ => InternalServerError()
                });
}