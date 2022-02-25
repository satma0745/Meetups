namespace Meetups.Domain.Entities.Meetup;

using System;
using Meetups.Domain.Seedwork;

public class MeetupPlace
{
    #region Validation

    private static void EnsureValidCity(City city) =>
        Assertions.EnsureValidObject(nameof(city), city, required: true);

    private static void EnsureValidAddress(string address) =>
        Assertions.EnsureValidString(nameof(address), address, required: true, maxLength: 75);

    #endregion
    
    #region State
    
    public City City { get; }
    
    public string Address { get; }
    
    #endregion

    #region Constructors

    public MeetupPlace(City city, string address)
        : this(address)
    {
        EnsureValidCity(city);

        City = city;
    }

    // Only for EF Core
    private MeetupPlace(string address)
    {
        EnsureValidAddress(address);

        Address = address;
    }

    #endregion
    
    #region Equality

    public override int GetHashCode() =>
        HashCode.Combine(City, Address);

    public override bool Equals(object obj) =>
        ReferenceEquals(obj, this) ||
        obj is MeetupPlace other &&
        other.City == City &&
        other.Address == Address;

    #endregion
}