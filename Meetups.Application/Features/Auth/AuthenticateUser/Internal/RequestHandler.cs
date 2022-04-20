namespace Meetups.Application.Features.Auth.AuthenticateUser.Internal;

using System;
using System.Threading.Tasks;
using BCrypt.Net;
using Meetups.Application.Modules.Auth;
using Meetups.Application.Modules.Persistence;
using Meetups.Application.Seedwork.Internal;
using Meetups.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;

public class RequestHandler : RequestHandlerBase<Request, Result, ErrorTypes>
{
    private readonly IApplicationContext context;
    private readonly ITokenHelper tokenHelper;

    public RequestHandler(IApplicationContext context, ITokenHelper tokenHelper)
    {
        this.context = context;
        this.tokenHelper = tokenHelper;
    }

    public override async Task<Response<Result, ErrorTypes>> HandleRequest(Request request)
    {
        var user = await context.Users
            .Include(user => user.RefreshTokens)
            .SingleOrDefaultAsync(user => user.Username == request.Username);
        if (user is null)
        {
            return Failure(ErrorTypes.UserDoesNotExist);
        }

        var passwordMatches = BCrypt.Verify(request.Password, user.Password);
        if (!passwordMatches)
        {
            return Failure(ErrorTypes.IncorrectPassword);
        }

        // Persist refresh token so it can be used later
        var persistedRefreshToken = new RefreshToken(tokenId: Guid.NewGuid(), bearerId: user.Id);
        user.AddRefreshToken(persistedRefreshToken);
        await context.SaveChangesAsync();

        var tokenPair = tokenHelper.IssueTokenPair(user, persistedRefreshToken.TokenId);
        var result = new Result(tokenPair.AccessToken, tokenPair.RefreshToken);
        return Success(result);
    }
}