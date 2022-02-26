namespace Meetups.Application.Features.Studio.RegisterNewMeetup.Internal;

public record Result
{
    public static readonly Result NoPayload = new();
    
    private Result()
    {
    }
}