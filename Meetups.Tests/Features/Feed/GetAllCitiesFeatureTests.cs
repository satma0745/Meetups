namespace Meetups.Tests.Features.Feed;

using System.Linq;
using System.Threading.Tasks;
using Meetups.Application.Features.Feed.GetAllCities.Internal;
using Meetups.Application.Modules.Persistence;
using Meetups.Domain.Entities.Meetup;
using Meetups.Tests.Seedwork;
using Xunit;

public class GetAllCitiesFeatureTests
{
    private readonly IApplicationContext context;
    private readonly RequestHandler requestHandler;

    public GetAllCitiesFeatureTests()
    {
        context = ContextFactory.InMemory();
        requestHandler = new RequestHandler(context);
    }

    [Fact]
    public async Task Success()
    {
        // Arrange

        var cities = new[]
        {
            new City("Oslo"),
            new City("Dallas"),
            new City("Austin")
        };
        context.Cities.AddRange(cities);
        await context.SaveChangesAsync();
        
        // Act

        var request = new Request();
        var response = await requestHandler.HandleRequest(request);
        
        // Assert
        
        Assert.True(response.Success);
        Assert.NotNull(response.Payload);
        Assert.Equal(cities.Length, response.Payload.Count);

        var pairs = cities
            .Join(
                response.Payload,
                city => city.Id,
                result => result.Id,
                (city, result) => (city, result))
            .ToList();
        Assert.Equal(cities.Length, pairs.Count);

        foreach (var (city, result) in pairs)
        {
            Assert.Equal(city.Id, result.Id);
            Assert.Equal(city.Name, result.Name);
        }
    }
}