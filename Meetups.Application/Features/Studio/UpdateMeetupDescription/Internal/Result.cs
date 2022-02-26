namespace Meetups.Application.Features.Studio.UpdateMeetupDescription.Internal;

public record Result
{
    public static readonly Result NoPayload = new();
    
    private Result()
    {
    }
}