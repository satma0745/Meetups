namespace Meetups.Application.Features.Studio.RegisterNewMeetup.Internal;

public class Result
{
    public static readonly Result NoPayload = new();
    
    private Result()
    {
    }
}