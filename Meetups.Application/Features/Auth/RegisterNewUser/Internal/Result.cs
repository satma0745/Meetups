namespace Meetups.Application.Features.Auth.RegisterNewUser.Internal;

public record Result
{
    public static readonly Result NoPayload = new();
    
    private Result()
    {
    }
}