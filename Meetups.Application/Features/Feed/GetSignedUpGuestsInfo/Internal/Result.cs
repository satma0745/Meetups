namespace Meetups.Application.Features.Feed.GetSignedUpGuestsInfo.Internal;

using System;
using System.Collections.Generic;

public class Result : List<GuestModel>
{
    public Result(IEnumerable<GuestModel> guests)
        : base(guests)
    {
    }
}

public class GuestModel
{
    public Guid Id { get; }
    
    public string DisplayName { get; }

    public GuestModel(Guid id, string displayName)
    {
        Id = id;
        DisplayName = displayName;
    }
}