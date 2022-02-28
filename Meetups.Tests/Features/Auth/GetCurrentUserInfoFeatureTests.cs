namespace Meetups.Tests.Features.Auth;

using System;
using System.Threading.Tasks;
using BCrypt.Net;
using Meetups.Application.Features.Auth.GetCurrentUserInfo.Internal;
using Meetups.Application.Modules.Persistence;
using Meetups.Domain.Entities.User;
using Meetups.Tests.Seedwork;
using Xunit;

public class GetCurrentUserInfoFeatureTests
{
    private readonly IApplicationContext context;
    private readonly RequestHandler requestHandler;

    public GetCurrentUserInfoFeatureTests()
    {
        context = ContextFactory.InMemory();
        requestHandler = new RequestHandler(context);
    }

    [Fact]
    public async Task Success()
    {
        // Arrange

        var targetUser = new Guest(
            username: "satma0745",
            password: BCrypt.HashPassword("pa$$word"),
            displayName: "Satma 0745");
        context.Users.Add(targetUser);
        await context.SaveChangesAsync();
        
        // Act

        var request = new Request(targetUser.Id);
        var response = await requestHandler.HandleRequest(request);
        
        // Assert
        
        Assert.True(response.Success);
        Assert.NotNull(response.Payload);

        var userInfo = response.Payload;
        Assert.Equal(targetUser.Id, userInfo.Id);
        Assert.Equal(targetUser.Username, userInfo.Username);
        Assert.Equal(targetUser.DisplayName, userInfo.DisplayName);
    }

    [Fact]
    public async Task FailureForNotExistingUser()
    {
        // Act

        var request = new Request(Guid.NewGuid());
        var response = await requestHandler.HandleRequest(request);
        
        // Assert
        
        Assert.False(response.Success);
        Assert.Equal(ErrorTypes.UserDoesNotExist, response.ErrorType);
    }
}