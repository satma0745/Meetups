namespace Meetups.Tests.Features.Auth;

using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;
using Meetups.Application.Features.Auth.AuthenticateUser.Internal;
using Meetups.Application.Modules.Auth;
using Meetups.Application.Modules.Persistence;
using Meetups.Domain.Entities.User;
using Meetups.Tests.Mocks;
using Meetups.Tests.Seedwork;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class AuthenticateUserFeatureTests
{
    private readonly ITokenHelper tokenHelper;
    private readonly IApplicationContext context;
    private readonly RequestHandler requestHandler;

    public AuthenticateUserFeatureTests()
    {
        tokenHelper = new TokenHelperMock();
        context = ContextFactory.InMemory();
        requestHandler = new RequestHandler(context, tokenHelper);
    }

    [Fact]
    public async Task Success()
    {
        // Arrange

        var user = new Guest(
            username: "satma0745",
            password: BCrypt.HashPassword("pa$$word"),
            displayName: "Satma 0745");
        context.Users.Add(user);
        await context.SaveChangesAsync();
        
        // Act

        var request = new Request(
            Username: "satma0745",
            Password: "pa$$word");
        var response = await requestHandler.HandleRequest(request);
        
        // Assert
        
        Assert.True(response.Success);
        Assert.NotNull(response.Payload);
        
        var persistedUser = await context.Users
            .Include(persistedUser => persistedUser.RefreshTokens)
            .SingleAsync();
        Assert.Equal(1, persistedUser.RefreshTokens.Count);
        var persistedRefreshToken = persistedUser.RefreshTokens.Single();
        
        var (_, refreshToken) = response.Payload;
        tokenHelper.TryParseRefreshToken(refreshToken, out var refreshTokenPayload);
        Assert.Equal(persistedRefreshToken.BearerId, refreshTokenPayload.BearerId);
        Assert.Equal(persistedRefreshToken.TokenId, refreshTokenPayload.TokenId);
    }

    [Fact]
    public async Task FailureForNotExistingUser()
    {
        // Act

        var request = new Request(
            Username: "Does not exist",
            Password: BCrypt.HashPassword("Does not matter"));
        var response = await requestHandler.HandleRequest(request);
        
        // Assert
        
        Assert.False(response.Success);
        Assert.Equal(ErrorTypes.UserDoesNotExist, response.ErrorType);
    }

    [Fact]
    public async Task FailureForIncorrectPassword()
    {
        // Arrange

        var user = new Guest(
            username: "satma0745",
            password: BCrypt.HashPassword("pa$$word"),
            displayName: "Satma 0745");
        context.Users.Add(user);
        await context.SaveChangesAsync();
        
        // Act

        var request = new Request(
            Username: "satma0745",
            Password: BCrypt.HashPassword("Incorrect password"));
        var response = await requestHandler.HandleRequest(request);
        
        // Assert
        
        Assert.False(response.Success);
        Assert.Equal(ErrorTypes.IncorrectPassword, response.ErrorType);
    }
}