namespace Meetups.Backend.Entities.Meetup;

using System;

public class MeetupDuration
{
    #region Validation

    private static void EnsureValidHours(int hours)
    {
        if (hours < 0)
        {
            throw new ArgumentException("Must not be a negative number.", nameof(hours));
        }
    }

    private static void EnsureValidMinutes(int minutes)
    {
        if (minutes < 0)
        {
            throw new ArgumentException("Must not be a negative number.", nameof(minutes));
        }
        if (minutes >= 60)
        {
            throw new ArgumentException("Must not be greater than or equal to 60.", nameof(minutes));
        }
    }
    
    #endregion
    
    #region State

    public int Hours { get; }
    
    public int Minutes { get; }

    #endregion

    #region Constructors

    public MeetupDuration(int hours, int minutes)
    {
        EnsureValidHours(hours);
        EnsureValidMinutes(minutes);
        
        Hours = hours;
        Minutes = minutes;
    }

    #endregion

    #region Equality

    public override int GetHashCode() =>
        Hours * 60 + Minutes;

    public override bool Equals(object obj) =>
        ReferenceEquals(obj, this) ||
        obj is MeetupDuration other &&
        other.GetHashCode() == GetHashCode();

    #endregion
}