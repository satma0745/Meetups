namespace Meetups.Backend.Entities.Meetup;

using System;
using System.Collections.Generic;
using Meetups.Backend.Entities.User;

public class Meetup
{
    #region State
    
    public Guid Id { get; }
    
    public string Topic { get; private set; }
    
    public string Place { get; private set; }
    
    public MeetupDuration Duration { get; private set; }
    
    public DateTime StartTime { get; private set; }

    public Organizer Organizer { get; set; }

    public ICollection<Guest> SignedUpGuests { get; set; }
    
    #endregion

    #region Constructor

    public Meetup(string topic, string place, MeetupDuration duration, DateTime startTime)
        : this(id: Guid.NewGuid(), topic, place, duration, startTime)
    {
    }

    private Meetup(Guid id, string topic, string place, MeetupDuration duration, DateTime startTime)
    {
        Id = id;
        Topic = topic;
        Place = place;
        Duration = duration;
        StartTime = startTime;
    }

    #endregion

    #region Behavior

    public void UpdateMeetupInfo(string topic, string place, MeetupDuration duration, DateTime startTime)
    {
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