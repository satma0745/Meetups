namespace Meetups.Backend.Persistence.Entities;

using System;

public class RefreshToken
{
    public Guid TokenId { get; set; }
    
    public Guid UserId { get; set; }
}