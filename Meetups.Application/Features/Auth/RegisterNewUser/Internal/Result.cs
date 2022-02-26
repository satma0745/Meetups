namespace Meetups.Application.Features.Auth.RegisterNewUser.Internal;

public class Result
{
    public static readonly Result NoPayload = new();
    
    private Result()
    {
    }
}