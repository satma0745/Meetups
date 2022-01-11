namespace Meetups.Persistence.Entities;

using System;
using System.ComponentModel.DataAnnotations;

public class RefreshToken
{
    [Key]
    public Guid TokenId { get; set; }
    
    public Guid UserId { get; set; }
}