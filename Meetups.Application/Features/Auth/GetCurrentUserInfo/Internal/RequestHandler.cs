namespace Meetups.Application.Features.Auth.GetCurrentUserInfo.Internal;

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
        var user = await context.Users.SingleOrDefaultAsync(user => user.Id == request.CurrentUserId);
        if (user is null)
        {
            return Failure(ErrorTypes.UserDoesNotExist);
        }

        var result = new Result(user.Id, user.Username, user.DisplayName);
        return Success(result);
    }
}