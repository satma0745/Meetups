namespace Meetups.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/ping-pong")]
public class HelloWorldController : ControllerBase
{
    [HttpGet]
    public IActionResult Ping() => Ok("Pong!");
}
