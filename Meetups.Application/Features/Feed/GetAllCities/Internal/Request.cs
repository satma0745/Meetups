namespace Meetups.Application.Features.Feed.GetAllCities.Internal;

public class Request
{
    public static readonly Request NoPayload = new();
    
    private Request()
    {
    }
}