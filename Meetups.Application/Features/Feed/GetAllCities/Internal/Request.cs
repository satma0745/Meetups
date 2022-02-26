namespace Meetups.Application.Features.Feed.GetAllCities.Internal;

public record Request
{
    public static readonly Request NoPayload = new();
    
    private Request()
    {
    }
}