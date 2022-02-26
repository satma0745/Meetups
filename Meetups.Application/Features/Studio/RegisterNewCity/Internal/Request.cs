namespace Meetups.Application.Features.Studio.RegisterNewCity.Internal;

public class Request
{
    public string Name { get; }

    public Request(string name) =>
        Name = name;
}