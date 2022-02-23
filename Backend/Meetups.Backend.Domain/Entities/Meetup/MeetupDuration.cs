namespace Meetups.Backend.Domain.Entities.Meetup;

using Meetups.Backend.Domain.Seedwork;

public class MeetupDuration
{
    #region Validation

    private static void EnsureValidHours(int hours) =>
        Assertions.EnsureValidNumber(nameof(hours), hours, minValue: 0);

    private static void EnsureValidMinutes(int minutes) =>
        Assertions.EnsureValidNumber(nameof(minutes), minutes, minValue: 0, maxValue: 59);

    private static void EnsureValidTotalMinutes(int totalMinutes) =>
        Assertions.EnsureValidNumber(nameof(totalMinutes), totalMinutes, minValue: 0);
    
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