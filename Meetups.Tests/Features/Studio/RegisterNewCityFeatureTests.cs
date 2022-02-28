namespace Meetups.Tests.Features.Studio;

using System;
using System.Threading.Tasks;
using Meetups.Application.Features.Studio.RegisterNewCity.Internal;
using Meetups.Application.Modules.Persistence;
using Meetups.Domain.Entities.Meetup;
using Meetups.Tests.Seedwork;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class RegisterNewCityFeatureTests
{
    private readonly IApplicationContext context;
    private readonly RequestHandler requestHandler;

    public RegisterNewCityFeatureTests()
    {
        context = ContextFactory.InMemory();
        requestHandler = new RequestHandler(context);
    }

    [Fact]
    public async Task Success()
    {
        // Act

        var request = new Request("Oslo");
        var response = await requestHandler.HandleRequest(request);
        
        // Assert
        
        Assert.True(response.Success);
        Assert.NotNull(response.Payload);

        var result = response.Payload;
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(request.Name, result.Name);

        var persisted = await context.Cities.SingleAsync();
        Assert.Equal(result.Id, persisted.Id);
        Assert.Equal(result.Name, persisted.Name);
    }
    
    [Fact]
    public async Task FailureForAlreadyTakenName()
    {
        // Arrange

        var oslo = new City("Oslo");
        context.Cities.Add(oslo);
        await context.SaveChangesAsync();
        
        // Act

        var request = new Request("Oslo");
        var response = await requestHandler.HandleRequest(request);
        
        // Assert
        
        Assert.False(response.Success);
        Assert.Equal(ErrorTypes.NameAlreadyTaken, response.ErrorType);
    }
}