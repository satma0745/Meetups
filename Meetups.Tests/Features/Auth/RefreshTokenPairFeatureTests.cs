namespace Meetups.Tests.Features.Auth;

using System;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;
using Meetups.Application.Features.Auth.RefreshTokenPair.Internal;
using Meetups.Application.Modules.Auth;
using Meetups.Application.Modules.Persistence;
using Meetups.Domain.Entities.User;
using Meetups.Tests.Mocks;
using Meetups.Tests.Seedwork;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class RefreshTokenPairFeatureTests
{
    private readonly ITokenHelper tokenHelper;
    private readonly IApplicationContext context;
    private readonly RequestHandler requestHandler;

    public RefreshTokenPairFeatureTests()
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
            username: "target_user",
            password: BCrypt.HashPassword("pa$$word"),
            displayName: "Target user");
        context.Users.Add(user);

        var otherPersistedRefreshToken = new RefreshToken(Guid.NewGuid(), user.Id);
        var oldPersistedRefreshToken = new RefreshToken(Guid.NewGuid(), user.Id);
        var oldRefreshToken = tokenHelper
            .IssueTokenPair(
                user: user,
                refreshTokenId: oldPersistedRefreshToken.TokenId)
            .RefreshToken;
        
        user.AddRefreshToken(otherPersistedRefreshToken);
        user.AddRefreshToken(oldPersistedRefreshToken);
        await context.SaveChangesAsync();
        
        // Act

        var request = new Request(oldRefreshToken);
        var response = await requestHandler.HandleRequest(request);
        
        // Assert
        
        Assert.True(response.Success);
        Assert.NotNull(response.Payload);

        var persistedUser = await context.Users
            .Include(persistedUser => persistedUser.RefreshTokens)
            .SingleAsync();
        var persistedRefreshTokens = persistedUser.RefreshTokens;
        Assert.Equal(2, persistedRefreshTokens.Count);
        Assert.Contains(otherPersistedRefreshToken, persistedRefreshTokens);

        var newPersistedRefreshToken = persistedRefreshTokens
            .SingleOrDefault(persistedRefreshToken => !persistedRefreshToken.Equals(otherPersistedRefreshToken));
        Assert.NotNull(newPersistedRefreshToken);
        
        var (_, newRefreshToken) = response.Payload;
        tokenHelper.TryParseRefreshToken(newRefreshToken, out var newRefreshTokenPayload);
        Assert.Equal(newRefreshTokenPayload.TokenId, newPersistedRefreshToken!.TokenId);
        Assert.Equal(newRefreshTokenPayload.BearerId, newPersistedRefreshToken.BearerId);
    }

    [Fact]
    public async Task FailureForInvalidRefreshTokenProvided()
    {
        // Act

        var request = new Request("invalid.refresh.token");
        var response = await requestHandler.HandleRequest(request);
        
        // Assert
        
        Assert.False(response.Success);
        Assert.Equal(ErrorTypes.InvalidRefreshTokenProvided, response.ErrorType);
    }

    [Fact]
    public async Task FailureForNotExistingTokenBearer()
    {
        // Arrange
        
        // user is not considered existing since we didn't add him to the IApplicationContext
        var notExistingUser = new Guest(
            username: "not existing",
            password: BCrypt.HashPassword("pa$$word"),
            displayName: "Not Existing");

        var refreshToken = tokenHelper
            .IssueTokenPair(
                user: notExistingUser,
                refreshTokenId: Guid.Empty)
            .RefreshToken;

        // Act

        var request = new Request(refreshToken);
        var response = await requestHandler.HandleRequest(request);
        
        // Assert

        Assert.False(response.Success);
        Assert.Equal(ErrorTypes.TokenBearerDoesNotExist, response.ErrorType);
    }

    [Fact]
    public async Task FailureForFakeOrUsedRefreshTokenProvided()
    {
        // Arrange

        var user = new Guest(
            username: "target_user",
            password: BCrypt.HashPassword("pa$$word"),
            displayName: "Target user");
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var fakeRefreshToken = tokenHelper
            .IssueTokenPair(
                user: user,
                refreshTokenId: Guid.NewGuid())
            .RefreshToken;
        
        // Act

        var request = new Request(fakeRefreshToken);
        var response = await requestHandler.HandleRequest(request);
        
        // Assert
        
        Assert.False(response.Success);
        Assert.Equal(ErrorTypes.FakeOrUsedRefreshTokenProvided, response.ErrorType);
    }
}