namespace Meetups.Application.Features.Auth.RefreshTokenPair.Internal;

using System;
using System.Threading.Tasks;
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
        if (!tokenHelper.TryParseRefreshToken(request.OldRefreshToken, out var oldRefreshTokenPayload))
        {
            return Failure(ErrorTypes.InvalidRefreshTokenProvided);
        }

        var currentUser = await context.Users
            .Include(user => user.RefreshTokens)
            .SingleOrDefaultAsync(user => user.Id == oldRefreshTokenPayload.BearerId);
        if (currentUser is null)
        {
            return Failure(ErrorTypes.TokenBearerDoesNotExist);
        }
        if (!currentUser.TryGetRefreshToken(oldRefreshTokenPayload.TokenId, out var oldPersistedRefreshToken))
        {
            return Failure(ErrorTypes.FakeOrUsedRefreshTokenProvided);
        }

        var newPersistedRefreshToken = new RefreshToken(tokenId: Guid.NewGuid(), oldRefreshTokenPayload.BearerId);
        currentUser.ReplaceRefreshToken(oldPersistedRefreshToken, newPersistedRefreshToken);
        await context.SaveChangesAsync();

        var tokenPair = tokenHelper.IssueTokenPair(currentUser, newPersistedRefreshToken.TokenId);
        var result = new Result(tokenPair.AccessToken, tokenPair.RefreshToken);
        return Success(result);
    }
}