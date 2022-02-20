namespace Meetups.Backend.Entities.Meetup;

public class MeetupDuration
{
    #region State

    public int Hours { get; }
    
    public int Minutes { get; }

    #endregion

    #region Constructors

    public MeetupDuration(int hours, int minutes)
    {
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