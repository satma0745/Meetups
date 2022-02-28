namespace Meetups.Tests.Features.Studio;

using System;
using System.Threading.Tasks;
using BCrypt.Net;
using Meetups.Application.Features.Studio.DeleteSpecificMeetup.Internal;
using Meetups.Application.Modules.Persistence;
using Meetups.Domain.Entities.Meetup;
using Meetups.Domain.Entities.User;
using Meetups.Tests.Seedwork;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class DeleteSpecificMeetupFeatureTests
{
    private readonly IApplicationContext context;
    private readonly RequestHandler requestHandler;

    public DeleteSpecificMeetupFeatureTests()
    {
        context = ContextFactory.InMemory();
        requestHandler = new RequestHandler(context);
    }

    [Fact]
    public async Task Success()
    {
        // Arrange

        var oslo = new City("Oslo");
        context.Cities.Add(oslo);

        var meetup = new Meetup(
            topic: "Some meetup",
            place: new MeetupPlace(
                city: oslo,
                address: "Some address"),
            startTime: DateTime.UtcNow,
            duration: new MeetupDuration(
                hours: 3,
                minutes: 0));
        context.Meetups.Add(meetup);

        var organizer = new Organizer(
            username: "organizer",
            password: BCrypt.HashPassword("pa$$word"),
            displayName: "Some meetup organizer");
        organizer.AddOrganizedMeetup(meetup);
        context.Organizers.Add(organizer);

        await context.SaveChangesAsync();
        
        // Act

        var request = new Request(
            MeetupId: meetup.Id,
            CurrentUserId: organizer.Id);
        var response = await requestHandler.HandleRequest(request);
        
        // Assert
        
        Assert.True(response.Success);

        var persistedMeetups = await context.Meetups.ToListAsync();
        Assert.Empty(persistedMeetups);
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
    public async Task FailureForAccessViolation()
    {
        // Arrange

        var oslo = new City("Oslo");
        context.Cities.Add(oslo);

        var meetup = new Meetup(
            topic: "Some meetup",
            place: new MeetupPlace(
                city: oslo,
                address: "Some address"),
            startTime: DateTime.UtcNow,
            duration: new MeetupDuration(
                hours: 3,
                minutes: 0));
        context.Meetups.Add(meetup);

        var organizer = new Organizer(
            username: "organizer",
            password: BCrypt.HashPassword("pa$$word"),
            displayName: "Meetup organizer");
        organizer.AddOrganizedMeetup(meetup);
        context.Organizers.Add(organizer);

        var targetUser = new Organizer(
            username: "target",
            password: BCrypt.HashPassword("pa$$word"),
            displayName: "Target user");
        context.Organizers.Add(targetUser);

        await context.SaveChangesAsync();
        
        // Act

        var request = new Request(
            MeetupId: meetup.Id,
            CurrentUserId: targetUser.Id);
        var response = await requestHandler.HandleRequest(request);
        
        // Assert
        
        Assert.False(response.Success);
        Assert.Equal(ErrorTypes.AccessViolation, response.ErrorType);
    }
}