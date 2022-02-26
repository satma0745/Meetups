namespace Meetups.Application.Features.Feed.SignUpForMeetup.Internal;

public class Result
{
    public static readonly Result NoPayload = new();
    
    private Result()
    {
    }
}