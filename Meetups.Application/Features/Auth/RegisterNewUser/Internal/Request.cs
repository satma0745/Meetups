namespace Meetups.Application.Features.Auth.RegisterNewUser.Internal;

public record Request(
    string Username,
    string Password,
    string DisplayName,
    string AccountType);