namespace Meetups.Tests.Features.Auth;

using System;
using System.Threading.Tasks;
using BCrypt.Net;
using Meetups.Application.Features.Auth.ChangeCredentials.Internal;
using Meetups.Application.Modules.Persistence;
using Meetups.Domain.Entities.User;
using Meetups.Tests.Seedwork;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class ChangeCredentialsFeatureTests
{
    private readonly IApplicationContext context;
    private readonly RequestHandler requestHandler;

    public ChangeCredentialsFeatureTests()
    {
        context = ContextFactory.InMemory();
        requestHandler = new RequestHandler(context);
    }

    [Fact]
    public async Task Success()
    {
        // Arrange

        var targetGuest = new Guest(
            username: "satma0745",
            password: BCrypt.HashPassword("pa$$word"),
            displayName: "Satma 0745");
        targetGuest.AddRefreshToken(new RefreshToken(Guid.NewGuid(), targetGuest.Id));
        
        context.Guests.Add(targetGuest);
        await context.SaveChangesAsync();
        
        // Act

        var request = new Request(
            UserId: targetGuest.Id,
            Username: "updated",
            Password: "updated");
        var response = await requestHandler.HandleRequest(request);
        
        // Assert
        
        Assert.True(response.Success);

        var persistedGuest = await context.Guests
            .Include(guest => guest.RefreshTokens)
            .SingleAsync();
        Assert.Equal(request.Username, persistedGuest.Username);
        Assert.True(BCrypt.Verify(request.Password, persistedGuest.Password));
        Assert.Empty(persistedGuest.RefreshTokens);
    }

    [Fact]
    public async Task FailureForTakenUsername()
    {
        // Arrange

        var targetGuest = new Guest(
            username: "Target guest",
            password: BCrypt.HashPassword("pa$$word"),
            displayName: "Target guest");
        var otherGuest = new Guest(
            username: "Taken username",
            password: BCrypt.HashPassword("pa$$word"),
            displayName: "Guest who took the desired username");
        context.Guests.AddRange(targetGuest, otherGuest);
        await context.SaveChangesAsync();
        
        // Act

        var request = new Request(
            UserId: targetGuest.Id,
            Username: "Taken username",
            Password: "pa$$word");
        var response = await requestHandler.HandleRequest(request);
        
        // Assert
        
        Assert.False(response.Success);
        Assert.Equal(ErrorTypes.UsernameAlreadyTaken, response.ErrorType);
    }
}