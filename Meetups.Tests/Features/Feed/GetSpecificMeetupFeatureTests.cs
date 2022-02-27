namespace Meetups.Tests.Features.Feed;

using System;
using System.Threading.Tasks;
using BCrypt.Net;
using Meetups.Application.Features.Feed.GetSpecificMeetup.Internal;
using Meetups.Application.Modules.Persistence;
using Meetups.Domain.Entities.Meetup;
using Meetups.Domain.Entities.User;
using Meetups.Tests.Seedwork;
using Xunit;

public class GetSpecificMeetupFeatureTests
{
    private readonly IApplicationContext context;
    private readonly RequestHandler requestHandler;

    public GetSpecificMeetupFeatureTests()
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

        var result = response.Payload;
        Assert.Equal(meetup.Id, result.Id);
        Assert.Equal(meetup.Topic, result.Topic);
        Assert.Equal(meetup.Place.City.Id, result.Place.CityId);
        Assert.Equal(meetup.Place.City.Name, result.Place.CityName);
        Assert.Equal(meetup.Place.Address, result.Place.Address);
        Assert.Equal(meetup.StartTime, result.StartTime);
        Assert.Equal(meetup.Duration.Hours, result.Duration.Hours);
        Assert.Equal(meetup.Duration.Minutes, result.Duration.Minutes);
        Assert.Equal(guests.Length, result.SignedUpGuestsCount);
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