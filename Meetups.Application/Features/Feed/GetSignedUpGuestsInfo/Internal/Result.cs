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

public record GuestModel(
    Guid Id,
    string DisplayName);