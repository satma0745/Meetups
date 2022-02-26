namespace Meetups.Application.Features.Studio.RegisterNewCity.Internal;

using System;

public class Result
{
    public Guid Id { get; }
    
    public string Name { get; }

    public Result(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}