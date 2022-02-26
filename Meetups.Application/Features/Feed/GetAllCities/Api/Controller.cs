namespace Meetups.Application.Features.Feed.GetAllCities.Api;

using System.Threading.Tasks;
using Meetups.Application.Features.Feed.GetAllCities.Internal;
using Meetups.Application.Features.Shared.Infrastructure.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[Tags("Feed")]
public class Controller : ApiControllerBase
{
    private readonly RequestHandler requestHandler;

    public Controller(RequestHandler requestHandler) =>
        this.requestHandler = requestHandler;

    /// <summary>Get all existing cities.</summary>
    /// <response code="200">Cities retrieved successfully.</response>
    [HttpGet("feed/all-cities")]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCities()
    {
        var internalResponse = await requestHandler.HandleRequest(Internal.Request.NoPayload);
        return (internalResponse.Success, internalResponse.Payload) switch
        {
            (true, var internalResult) => Ok(internalResult.ToApiResponse()),
            (false, _) => InternalServerError()
        };
    }
}