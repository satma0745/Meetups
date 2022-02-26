namespace Meetups.Application.Features.Studio.RegisterNewCity.Internal;

using System;

public record Result(
    Guid Id,
    string Name);