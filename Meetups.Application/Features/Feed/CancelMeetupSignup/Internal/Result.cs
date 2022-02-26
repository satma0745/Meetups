namespace Meetups.Application.Features.Feed.CancelMeetupSignup.Internal;

public record Result
{
    public static readonly Result NoPayload = new();
    
    private Result()
    {
    }
}