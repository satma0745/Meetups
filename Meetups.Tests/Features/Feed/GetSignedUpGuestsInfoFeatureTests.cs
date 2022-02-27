namespace Meetups.Tests.Features.Feed;

using System;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;
using Meetups.Application.Features.Feed.GetSignedUpGuestsInfo.Internal;
using Meetups.Application.Modules.Persistence;
using Meetups.Domain.Entities.Meetup;
using Meetups.Domain.Entities.User;
using Meetups.Tests.Seedwork;
using Xunit;

public class GetSignedUpGuestsInfoFeatureTests
{
    private readonly IApplicationContext context;
    private readonly RequestHandler requestHandler;

    public GetSignedUpGuestsInfoFeatureTests()
    {
        context = ContextFactory.InMemory();
        requestHandler = new RequestHandler(context);
    }

    [Fact]
    public async Task Success()
    {
        // Arrange

        var oslo = new City("Oslo");
        var meetup = new Meetup(
            topic: "Some meetup",
            place: new MeetupPlace(oslo, address: "Some address"),
            duration: new MeetupDuration(hours: 3, minutes: 30),
            startTime: DateTime.UtcNow);
        context.Meetups.Add(meetup);
        
        var organizer = new Organizer(
            username: "organizer",
            password: BCrypt.HashPassword("pa$$word"),
            displayName: "Some meetup organizer");
        organizer.AddOrganizedMeetup(meetup);
        context.Users.Add(organizer);

        var guests = new[]
        {
            new Guest(
                username: "Guest #1",
                password: BCrypt.HashPassword("pa$$word"),
                displayName: "Guest #1"),
            new Guest(
                username: "Guest #2",
                password: BCrypt.HashPassword("pa$$word"),
                displayName: "Guest #2")
        };
        foreach (var guest in guests)
        {
            guest.SignUpFor(meetup);
        }
        context.Guests.AddRange(guests);

        await context.SaveChangesAsync();
        
        // Act

        var request = new Request(meetup.Id);
        var response = await requestHandler.HandleRequest(request);
        
        // Assert
        
        Assert.True(response.Success);
        Assert.NotNull(response.Payload);
        Assert.Equal(guests.Length, response.Payload.Count);

        var pairs = guests
            .Join(
                response.Payload,
                guest => guest.Id,
                result => result.Id,
                (guest, result) => (guest, result))
            .ToList();
        Assert.Equal(guests.Length, pairs.Count);
        
        foreach (var (guest, result) in pairs)
        {
            Assert.Equal(guest.Id, result.Id);
            Assert.Equal(guest.DisplayName, result.DisplayName);
        }
    }

    [Fact]
    public async Task FailureForNotExistingMeetup()
    {
        // Act

        var request = new Request(Guid.NewGuid());
        var response = await requestHandler.HandleRequest(request);
        
        // Assert
        
        Assert.False(response.Success);
        Assert.Equal(ErrorTypes.MeetupDoesNotExist, response.ErrorType);
    }
}