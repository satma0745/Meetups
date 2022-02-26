namespace Meetups.Application.Features.Auth.SignOutEverywhere.Internal;

public record Result
{
    public static readonly Result NoPayload = new();
    
    private Result()
    {
    }
}