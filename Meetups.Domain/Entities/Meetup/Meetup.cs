namespace Meetups.Domain.Entities.Meetup;

using System;
using System.Collections.Generic;
using Meetups.Domain.Entities.User;
using Meetups.Domain.Seedwork;

public class Meetup
{
    #region Validation

    private static void EnsureValidId(Guid id) =>
        Assertions.EnsureValidGuid(nameof(id), id, required: true);

    private static void EnsureValidTopic(string topic) =>
        Assertions.EnsureValidString(nameof(topic), topic, maxLength: 100);

    private static void EnsureValidPlace(MeetupPlace place) =>
        Assertions.EnsureValidObject(nameof(place), place, required: true);

    private static void EnsureValidDuration(MeetupDuration duration) =>
        Assertions.EnsureValidObject(nameof(duration), duration, required: true);

    private static void EnsureValidStartTime(DateTime startTime) =>
        Assertions.None(nameof(startTime), startTime);

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

    public void Reschedule(MeetupPlace place, MeetupDuration duration, DateTime startTime)
    {
        EnsureValidPlace(place);
        EnsureValidDuration(duration);
        EnsureValidStartTime(startTime);

        Place = place;
        Duration = duration;
        StartTime = startTime;
    }

    public void UpdateDescription(string topic)
    {
        EnsureValidTopic(topic);

        Topic = topic;
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