namespace Meetups.Application.Features.Shared.Contracts.Auth;

public class AccessTokenPayload
{
    public const string BearerIdClaim = "sub";
    public const string UserRoleClaim = "role";
}