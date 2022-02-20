namespace Meetups.Backend.Application.Features.Auth.ChangeCredentials;

using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BCrypt.Net;
using Meetup.Contract.Models.Features.Auth.ChangeCredentials;
using Meetup.Contract.Routing;
using Meetups.Backend.Application.Seedwork;
using Meetups.Backend.Persistence.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Tags(Tags.Auth)]
public class Controller : ApiControllerBase
{
    public Controller(ApplicationContext context, IMapper mapper)
        : base(context, mapper)
    {
    }

    /// <summary>Change current user signing credentials.</summary>
    /// <param name="request">New credentials.</param>
    /// <response code="200">Credentials changed successfully.</response>
    /// <response code="409">Username already taken.</response>
    [Authorize]
    [HttpPut(Routes.Auth.ChangeCredentials)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ChangeCredentials([FromBody] RequestDto request)
    {
        var usernameTaken = await Context.Users
            .Where(user => user.Id != CurrentUser.UserId)
            .AnyAsync(user => user.Username == request.Username);
        if (usernameTaken)
        {
            return Conflict();
        }
        
        // Change credentials
        var user = await Context.Users.SingleAsync(user => user.Id == CurrentUser.UserId);
        user.Username = request.Username;
        user.Password = BCrypt.HashPassword(request.Password);

        // Revoke refresh tokens
        var userRefreshTokens = await Context.RefreshTokens
            .AsNoTracking()
            .Where(refreshToken => refreshToken.UserId == user.Id)
            .ToListAsync();
        Context.RefreshTokens.RemoveRange(userRefreshTokens);
        
        await Context.SaveChangesAsync();
        
        return Ok();
    }
}