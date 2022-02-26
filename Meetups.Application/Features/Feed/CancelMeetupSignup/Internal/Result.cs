namespace Meetups.Application.Features.Feed.CancelMeetupSignup.Internal;

public class Result
{
    public static readonly Result NoPayload = new();
    
    private Result()
    {
    }
}