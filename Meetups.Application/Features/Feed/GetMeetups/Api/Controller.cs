namespace Meetups.Application.Features.Feed.GetMeetups.Api;

using System.Threading.Tasks;
using Meetups.Application.Features.Feed.GetMeetups.Internal;
using Meetups.Application.Features.Shared.Infrastructure.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[Tags("Feed")]
public class Controller : ApiControllerBase
{
    private readonly RequestHandler requestHandler;

    public Controller(RequestHandler requestHandler) =>
        this.requestHandler = requestHandler;

    /// <summary>Get several meetups.</summary>
    /// <response code="200">Retrieved meetups successfully.</response>
    /// <response code="400">Validation failed for pagination options.</response>
    [HttpGet("feed")]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllMeetups([FromQuery] RequestDto requestDto)
    {
        var internalRequest = requestDto.ToInternalRequest();
        var internalResponse = await requestHandler.HandleRequest(internalRequest);
        return (internalResponse.Success, internalResponse.Payload) switch
        {
            (true, var internalResult) => Ok(internalResult.ToApiResponse()),
            (false, _) => InternalServerError()
        };
    }
}