namespace Meetups.Tests.Features.Auth;

using System.Threading.Tasks;
using BCrypt.Net;
using Meetups.Application.Features.Auth.SignOutEverywhere.Internal;
using Meetups.Application.Modules.Persistence;
using Meetups.Domain.Entities.User;
using Meetups.Tests.Seedwork;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class SignOutEverywhereFeatureTests
{
    private readonly IApplicationContext context;
    private readonly RequestHandler requestHandler;

    public SignOutEverywhereFeatureTests()
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

        var persistedUser = await context.Users
            .Include(user => user.RefreshTokens)
            .SingleAsync();
        Assert.Empty(persistedUser.RefreshTokens);
    }
}