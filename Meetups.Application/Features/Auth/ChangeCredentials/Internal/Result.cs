namespace Meetups.Application.Features.Auth.ChangeCredentials.Internal;

public record Result
{
    public static readonly Result NoPayload = new();
    
    private Result()
    {
    }
}