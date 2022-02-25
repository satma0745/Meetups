namespace Meetups.Application.Modules.Auth;

public interface ITokenPair
{
    string AccessToken { get; }
    
    string RefreshToken { get; }
}