namespace Meetups.Application.Features.Auth.SignOutEverywhere.Internal;

using System.Threading.Tasks;
using Meetups.Application.Features.Shared.Infrastructure.Internal;
using Meetups.Application.Modules.Persistence;
using Microsoft.EntityFrameworkCore;

public class RequestHandler : RequestHandlerBase<Request, Result, ErrorTypes>
{
    private readonly IApplicationContext context;

    public RequestHandler(IApplicationContext context) =>
        this.context = context;
    
    public override async Task<Response<Result, ErrorTypes>> HandleRequest(Request request)
    {
        var currentUser = await context.Users
            .Include(user => user.RefreshTokens)
            .SingleAsync(user => user.Id == request.CurrentUserId);
        
        currentUser.RevokeAllRefreshTokens();
        await context.SaveChangesAsync();

        return Success(Result.NoPayload);
    }
}