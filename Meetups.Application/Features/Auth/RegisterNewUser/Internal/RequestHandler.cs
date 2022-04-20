namespace Meetups.Application.Features.Auth.RegisterNewUser.Internal;

using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BCrypt.Net;
using Meetups.Application.Features.Shared.Auth;
using Meetups.Application.Modules.Persistence;
using Meetups.Application.Seedwork.Internal;
using Meetups.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;

public class RequestHandler : RequestHandlerBase<Request, Result, ErrorTypes>
{
    private readonly IApplicationContext context;

    public RequestHandler(IApplicationContext context) =>
        this.context = context;

    public override async Task<Response<Result, ErrorTypes>> HandleRequest(Request request)
    {
        var usernameTaken = await context.Users.AnyAsync(user => user.Username == request.Username);
        if (usernameTaken)
        {
            return Failure(ErrorTypes.UsernameAlreadyTaken);
        }

        var password = BCrypt.HashPassword(request.Password);
        User user = request.AccountType switch
        {
            UserRoles.Guest => new Guest(request.Username, password, request.DisplayName),
            UserRoles.Organizer => new Organizer(request.Username, password, request.DisplayName),
            var unmatched => throw new SwitchExpressionException(unmatched)
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return Success(Result.NoPayload);
    }
}