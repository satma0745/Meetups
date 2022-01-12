namespace Meetups.Features.User.GetCurrentUserInfo;

using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Meetups.Features.Shared;
using Meetups.Persistence.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Tags("User")]
public class Controller : ApiControllerBase
{
    public Controller(ApplicationContext context, IMapper mapper)
        : base(context, mapper)
    {
    }
    
    /// <summary>Retrieve info about the current user.</summary>
    /// <response code="200">Information was retrieved successfully.</response>
    /// <response code="401">Unauthorized request.</response>
    [Authorize]
    [HttpGet("users/who-am-i")]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCurrentUserInfo()
    {
        var currentUserIdClaim = User.Claims.Single(claim => claim.Type == "sub");
        var currentUserId = Guid.Parse(currentUserIdClaim.Value);

        var user = await Context.Users.SingleOrDefaultAsync(user => user.Id == currentUserId);
        if (user is null)
        {
            return Unauthorized();
        }

        var dto = Mapper.Map<ResponseDto>(user);
        return Ok(dto);
    }
}