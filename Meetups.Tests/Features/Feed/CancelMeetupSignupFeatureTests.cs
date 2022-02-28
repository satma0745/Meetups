namespace Meetups.Tests.Features.Feed;

using System;
using BCrypt.Net;
using System.Threading.Tasks;
using Meetups.Application.Features.Feed.CancelMeetupSignup.Internal;
using Meetups.Application.Modules.Persistence;
using Meetups.Domain.Entities.Meetup;
using Meetups.Domain.Entities.User;
using Meetups.Tests.Seedwork;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class CancelMeetupSignupFeatureTests
{
    private readonly IApplicationContext context;
    private readonly RequestHandler requestHandler;

    public CancelMeetupSignupFeatureTests()
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
        context.Organizers.Add(organizer);

        var guest = new Guest(
            username: "Target guest",
            password: BCrypt.HashPassword("pa$$word"),
            displayName: "Target guest");
        guest.SignUpFor(meetup);
        context.Guests.Add(guest);

        await context.SaveChangesAsync();
        
        // Act

        var request = new Request(
            MeetupId: meetup.Id,
            CurrentUserId: guest.Id);
        var response = await requestHandler.HandleRequest(request);
        
        // Assert
        
        Assert.True(response.Success);

        var persistedMeetup = await context.Meetups
            .Include(persistedMeetup => persistedMeetup.SignedUpGuests)
            .SingleAsync();
        Assert.Empty(persistedMeetup.SignedUpGuests);
    }

    [Fact]
    public async Task FailureForNotExistingMeetup()
    {
        // Act

        var request = new Request(
            MeetupId: Guid.NewGuid(),
            CurrentUserId: Guid.Empty);
        var response = await requestHandler.HandleRequest(request);
        
        // Assert
        
        Assert.False(response.Success);
        Assert.Equal(ErrorTypes.MeetupDoesNotExist, response.ErrorType);
    }

    [Fact]
    public async Task FailureWhenNotSignedUp()
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
        context.Organizers.Add(organizer);

        var guest = new Guest(
            username: "Target guest",
            password: BCrypt.HashPassword("pa$$word"),
            displayName: "Target guest");
        context.Guests.Add(guest);

        await context.SaveChangesAsync();
        
        // Act

        var request = new Request(
            MeetupId: meetup.Id,
            CurrentUserId: guest.Id);
        var response = await requestHandler.HandleRequest(request);
        
        // Assert
        
        Assert.False(response.Success);
        Assert.Equal(ErrorTypes.NotSignedUp, response.ErrorType);
    }
}