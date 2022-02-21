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

    private static void EnsureValidTotalMinutes(int totalMinutes)
    {
        if (totalMinutes < 0)
        {
            throw new ArgumentException("Must not be a negative number.", nameof(totalMinutes));
        }
    }
    
    #endregion
    
    #region State

    public int Hours { get; }
    
    public int Minutes { get; }

    public int TotalMinutes =>
        Hours * 60 + Minutes;

    #endregion

    #region Constructors

    public static MeetupDuration FromMinutes(int totalMinutes)
    {
        EnsureValidTotalMinutes(totalMinutes);
        
        return new MeetupDuration(totalMinutes / 60, totalMinutes % 60);
    }
    
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
        TotalMinutes;

    public override bool Equals(object obj) =>
        ReferenceEquals(obj, this) ||
        obj is MeetupDuration other &&
        other.TotalMinutes == TotalMinutes;

    #endregion
}