namespace Meetups.Application.Features.Auth.SignOutEverywhere.Internal;

public class Result
{
    public static readonly Result NoPayload = new();
    
    private Result()
    {
    }
}