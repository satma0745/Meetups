namespace Meetups.Features.DataTransferObjects.User;

public class TokenPairDto
{
    /// <summary>Short-living token used for user authorization.</summary>
    public string AccessToken { get; set; }
    
    /// <summary>Long-living persisted token used to obtain new access tokens.</summary>
    public string RefreshToken { get; set; }
}