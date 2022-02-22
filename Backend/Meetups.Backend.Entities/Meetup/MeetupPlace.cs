namespace Meetups.Backend.Entities.Meetup;

using System;

public class MeetupPlace
{
    #region Validation

    private static void EnsureValidCity(City city)
    {
        if (city is null)
        {
            throw new ArgumentException("Must not be null.", nameof(city));
        }
    }
    
    private static void EnsureValidAddress(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            throw new ArgumentException("Must not be null or empty.", nameof(address));
        }

        const int maxLength = 75;
        if (address.Length > maxLength)
        {
            throw new ArgumentException($"Must not exceed {maxLength} characters.", nameof(address));
        }
    }

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