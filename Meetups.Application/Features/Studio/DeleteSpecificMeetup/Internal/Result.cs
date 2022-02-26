namespace Meetups.Application.Features.Studio.DeleteSpecificMeetup.Internal;

public record Result
{
    public static readonly Result NoPayload = new();
    
    private Result()
    {
    }
}