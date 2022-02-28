namespace Meetups.Tests.Features.Auth;

using System.Threading.Tasks;
using BCrypt.Net;
using Meetups.Application.Features.Auth.RegisterNewUser.Internal;
using Meetups.Application.Features.Shared.Contracts.Auth;
using Meetups.Application.Modules.Persistence;
using Meetups.Domain.Entities.User;
using Meetups.Tests.Seedwork;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class RegisterNewUserFeatureTests
{
    private readonly IApplicationContext context;
    private readonly RequestHandler requestHandler;

    public RegisterNewUserFeatureTests()
    {
        context = ContextFactory.InMemory();
        requestHandler = new RequestHandler(context);
    }

    [Fact]
    public async Task SuccessForGuest()
    {
        // Act
        
        var request = new Request(
            Username: "satma0745",
            Password: "pa$$word",
            DisplayName: "Satma 0745",
            AccountType: UserRoles.Guest);
        var response = await requestHandler.HandleRequest(request);
        
        // Assert
        
        Assert.True(response.Success);
        Assert.NotNull(response.Payload);

        var savedUser = await context.Users.SingleOrDefaultAsync();
        Assert.NotNull(savedUser);
        
        Assert.Equal(request.Username, savedUser!.Username);
        Assert.True(BCrypt.Verify(request.Password, savedUser.Password));
        Assert.Equal(request.DisplayName, savedUser.DisplayName);
        Assert.True(savedUser is Guest);
    }
    
    [Fact]
    public async Task SuccessForOrganizer()
    {
        // Act
        
        var request = new Request(
            Username: "satma0745",
            Password: "pa$$word",
            DisplayName: "Satma 0745",
            AccountType: UserRoles.Organizer);
        var response = await requestHandler.HandleRequest(request);
        
        // Assert
        
        Assert.True(response.Success);
        Assert.NotNull(response.Payload);

        var savedUser = await context.Users.SingleOrDefaultAsync();
        Assert.NotNull(savedUser);
        
        Assert.Equal(request.Username, savedUser!.Username);
        Assert.True(BCrypt.Verify(request.Password, savedUser.Password));
        Assert.Equal(request.DisplayName, savedUser.DisplayName);
        Assert.True(savedUser is Organizer);
    }

    [Fact]
    public async Task FailureForTakenUsername()
    {
        // Arrange

        var existingGuest = new Guest(
            username: "Taken username",
            password: BCrypt.HashPassword("pa$$word"),
            displayName: "Some guest");
        context.Guests.Add(existingGuest);
        await context.SaveChangesAsync();
        
        // Act
        
        var request = new Request(
            Username: "Taken username",
            Password: "pa$$word",
            DisplayName: "Some guest #2",
            AccountType: UserRoles.Guest);
        var response = await requestHandler.HandleRequest(request);
        
        // Assert
        
        Assert.False(response.Success);
        Assert.Equal(ErrorTypes.UsernameAlreadyTaken, response.ErrorType);
    }
}