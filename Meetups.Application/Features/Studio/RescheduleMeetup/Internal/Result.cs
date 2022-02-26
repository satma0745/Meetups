namespace Meetups.Application.Features.Studio.RescheduleMeetup.Internal;

public record Result
{
    public static readonly Result NoPayload = new();
    
    private Result()
    {
    }
}