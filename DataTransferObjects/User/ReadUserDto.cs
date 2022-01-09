namespace Meetups.DataTransferObjects.User;

using System;

public class ReadUserDto
{
    /// <summary>Permanent unique user identifier.</summary>
    /// <example>07450745-0745-0745-0745-074507450745</example>
    public Guid Id { get; set; }
    
    /// <summary>Keyword used for authentication.</summary>
    /// <remarks>Also may be used as unique identifier.</remarks>
    /// <example>satma0745</example>
    public string Username { get; set; }
    
    /// <summary>Human readable public (official) name.</summary>
    /// <example>Satttttttttttttttter</example>
    public string DisplayName { get; set; }
}
