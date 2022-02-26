namespace Meetups.Application.Features.Feed.SignUpForMeetup.Internal;

public record Result
{
    public static readonly Result NoPayload = new();
    
    private Result()
    {
    }
}