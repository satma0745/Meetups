namespace Meetups.Application.Features.Feed.GetMeetups.Api;

using System.Runtime.CompilerServices;
using Meetups.Application.Features.Feed.GetMeetups.Internal;

internal static class Mappings
{
    public static OrderingOption ToOrderingOption(string orderBy) =>
        orderBy switch
        {
            OrderingOptions.TopicAlphabetically => OrderingOption.TopicAlphabetically,
            OrderingOptions.TopicReverseAlphabetically => OrderingOption.TopicReverseAlphabetically,
            OrderingOptions.DurationAscending => OrderingOption.DurationAscending,
            OrderingOptions.DurationDescending => OrderingOption.DurationDescending,
            OrderingOptions.SignUpsCountAscending => OrderingOption.SignUpsCountAscending,
            OrderingOptions.SignUpsCountDescending => OrderingOption.SignUpsCountDescending,
            var unmatched => throw new SwitchExpressionException(unmatched)
        };
}