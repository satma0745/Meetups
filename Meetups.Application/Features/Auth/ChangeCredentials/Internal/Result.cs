namespace Meetups.Application.Features.Auth.ChangeCredentials.Internal;

public class Result
{
    public static readonly Result NoPayload = new();
    
    private Result()
    {
    }
}