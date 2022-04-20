namespace Meetups.Application.Features.Auth.ChangeCredentials.Internal;

using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;
using Meetups.Application.Modules.Persistence;
using Meetups.Application.Seedwork.Internal;
using Microsoft.EntityFrameworkCore;

public class RequestHandler : RequestHandlerBase<Request, Result, ErrorTypes>
{
    private readonly IApplicationContext context;

    public RequestHandler(IApplicationContext context) =>
        this.context = context;
    
    public override async Task<Response<Result, ErrorTypes>> HandleRequest(Request request)
    {
        var usernameTaken = await context.Users
            .Include(user => user.RefreshTokens)
            .Where(user => user.Id != request.UserId)
            .AnyAsync(user => user.Username == request.Username);
        if (usernameTaken)
        {
            return Failure(ErrorTypes.UsernameAlreadyTaken);
        }
        
        var user = await context.Users.SingleAsync(user => user.Id == request.UserId);
        user.ChangeCredentials(request.Username, BCrypt.HashPassword(request.Password));
        user.RevokeAllRefreshTokens();
        await context.SaveChangesAsync();
        
        return Success(Result.NoPayload);
    }
}