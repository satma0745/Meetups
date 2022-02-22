namespace Meetups.Backend.Entities.Meetup;

using System;
using System.Collections.Generic;
using Meetups.Backend.Entities.User;

public class Meetup
{
    #region Validation

    private static void EnsureValidId(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Must not be empty.", nameof(id));
        }
    }

    private static void EnsureValidTopic(string topic)
    {
        if (string.IsNullOrWhiteSpace(topic))
        {
            throw new ArgumentException("Must not be null or empty.", nameof(topic));
        }

        const int maxLength = 100;
        if (topic.Length > maxLength)
        {
            throw new ArgumentException($"Must not exceed {maxLength} characters.", nameof(topic));
        }
    }

    private static void EnsureValidPlace(MeetupPlace place)
    {
        if (place is null)
        {
            throw new ArgumentException("Must not be null.", nameof(place));
        }
    }

    private static void EnsureValidDuration(MeetupDuration duration)
    {
        if (duration is null)
        {
            throw new ArgumentException("Must not be null.", nameof(duration));
        }
    }

    // This method does nothing right now, but may come in handy in the future
    // ReSharper disable once UnusedParameter.Local
    private static void EnsureValidStartTime(DateTime startTime)
    {
    }

    #endregion
    
    #region State
    
    public Guid Id { get; }
    
    public string Topic { get; private set; }
    
    public MeetupPlace Place { get; private set; }
    
    public MeetupDuration Duration { get; private set; }
    
    public DateTime StartTime { get; private set; }

    // Populated by the EF Core automatically when .Include is called
    // ReSharper disable once UnassignedGetOnlyAutoProperty
    public Organizer Organizer { get; }

    // Populated by the EF Core automatically when .Include is called
    // ReSharper disable once UnassignedGetOnlyAutoProperty
    public IReadOnlyCollection<Guest> SignedUpGuests { get; }

    #endregion

    #region Constructor

    public Meetup(string topic, MeetupPlace place, MeetupDuration duration, DateTime startTime)
        : this(id: Guid.NewGuid(), topic, duration, startTime)
    {
        EnsureValidPlace(place);
        
        Place = place;
    }
    
    // Only for EF Core
    private Meetup(Guid id, string topic, MeetupDuration duration, DateTime startTime)
    {
        EnsureValidId(id);
        EnsureValidTopic(topic);
        EnsureValidDuration(duration);
        EnsureValidStartTime(startTime);
        
        Id = id;
        Topic = topic;
        Duration = duration;
        StartTime = startTime;
    }

    #endregion

    #region Behavior

    public void UpdateMeetupInfo(string topic, MeetupPlace place, MeetupDuration duration, DateTime startTime)
    {
        EnsureValidTopic(topic);
        EnsureValidPlace(place);
        EnsureValidDuration(duration);
        EnsureValidStartTime(startTime);
        
        Topic = topic;
        Place = place;
        Duration = duration;
        StartTime = startTime;
    }

    #endregion
    
    #region Equality

    public override int GetHashCode() =>
        Id.GetHashCode();

    public override bool Equals(object obj) =>
        ReferenceEquals(obj, this) ||
        obj is Meetup other &&
        other.Id == Id &&
        other.Topic == Topic &&
        other.Place == Place &&
        other.Duration == Duration &&
        other.StartTime == StartTime;

    #endregion
}