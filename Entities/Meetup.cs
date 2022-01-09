﻿namespace Meetups.Entities;

using System;

public class Meetup
{
    public Guid Id { get; set; }
    public string Topic { get; set; }
    public string Place { get; set; }
    public TimeSpan Duration { get; set; }
    public DateTime StartTime { get; set; }
}
