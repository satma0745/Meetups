namespace Meetups.Tests.Features.Studio;

using System;
using System.Threading.Tasks;
using BCrypt.Net;
using Meetups.Application.Features.Studio.RescheduleMeetup.Internal;
using Meetups.Application.Modules.Persistence;
using Meetups.Domain.Entities.Meetup;
using Meetups.Domain.Entities.User;
using Meetups.Tests.Seedwork;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class RescheduleMeetupFeatureTests
{
    private readonly IApplicationContext context;
    private readonly RequestHandler requestHandler;

    public RescheduleMeetupFeatureTests()
    {
        context = ContextFactory.InMemory();
        requestHandler = new RequestHandler(context);
    }

    [Fact]
    public async Task Success()
    {
        // Arrange

        var oslo = new City("Oslo");
        var austin = new City("Austin");
        context.Cities.AddRange(oslo, austin);

        var meetup = new Meetup(
            topic: "Some meetup",
            place: new MeetupPlace(
                city: oslo,
                address: "Some address"),
            startTime: DateTime.UtcNow,
            duration: new MeetupDuration(
                hours: 3,
                minutes: 30));
        context.Meetups.Add(meetup);

        var organizer = new Organizer(
            username: "organizer",
            password: BCrypt.HashPassword("pa$$word"),
            displayName: "Some organizer");
        organizer.AddOrganizedMeetup(meetup);
        context.Organizers.Add(organizer);

        await context.SaveChangesAsync();
        
        // Act

        var request = new Request(
            MeetupId: meetup.Id,
            Place: new MeetupPlaceModel(
                CityId: austin.Id,
                Address: "Some new address"),
            StartTime: DateTime.UtcNow.AddDays(3),
            Duration: new MeetupDurationModel(
                Hours: 2,
                Minutes: 45),
            CurrentUserId: organizer.Id);
        var response = await requestHandler.HandleRequest(request);
        
        // Assert
        
        Assert.True(response.Success);

        var persisted = await context.Meetups
            .Include(persistedMeetup => persistedMeetup.Place.City)
            .SingleAsync();
        Assert.Equal(request.Place.CityId, persisted.Place.City.Id);
        Assert.Equal(request.Place.Address, persisted.Place.Address);
        Assert.Equal(request.StartTime, persisted.StartTime);
        Assert.Equal(request.Duration.Hours, persisted.Duration.Hours);
        Assert.Equal(request.Duration.Minutes, persisted.Duration.Minutes);
    }

    [Fact]
    public async Task FailureForNotExistingMeetup()
    {
        // Act

        var request = new Request(
            MeetupId: Guid.NewGuid(),
            Place: new MeetupPlaceModel(
                CityId: Guid.Empty,
                Address: "Does not matter"),
            StartTime: DateTime.UtcNow,
            Duration: new MeetupDurationModel(
                Hours: 2,
                Minutes: 15),
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
        var austin = new City("Austin");
        context.Cities.AddRange(oslo, austin);

        var meetup = new Meetup(
            topic: "Some meetup",
            place: new MeetupPlace(
                city: oslo,
                address: "Some address"),
            startTime: DateTime.UtcNow,
            duration: new MeetupDuration(
                hours: 3,
                minutes: 30));
        context.Meetups.Add(meetup);

        var meetupOrganizer = new Organizer(
            username: "organizer",
            password: BCrypt.HashPassword("pa$$word"),
            displayName: "Meetup organizer");
        meetupOrganizer.AddOrganizedMeetup(meetup);
        context.Organizers.Add(meetupOrganizer);

        var targetUser = new Organizer(
            username: "target",
            password: BCrypt.HashPassword("pa$$word"),
            displayName: "Target user");
        context.Organizers.Add(targetUser);

        await context.SaveChangesAsync();
        
        // Act

        var request = new Request(
            MeetupId: meetup.Id,
            Place: new MeetupPlaceModel(
                CityId: austin.Id,
                Address: "Some new address"),
            StartTime: DateTime.UtcNow.AddDays(3),
            Duration: new MeetupDurationModel(
                Hours: 2,
                Minutes: 45),
            CurrentUserId: targetUser.Id);
        var response = await requestHandler.HandleRequest(request);
        
        // Assert
        
        Assert.False(response.Success);
        Assert.Equal(ErrorTypes.AccessViolation, response.ErrorType);
    }

    [Fact]
    public async Task FailureForNotExistingCity()
    {
        // Arrange

        var oslo = new City("Oslo");
        context.Cities.AddRange(oslo);

        var meetup = new Meetup(
            topic: "Some meetup",
            place: new MeetupPlace(
                city: oslo,
                address: "Some address"),
            startTime: DateTime.UtcNow,
            duration: new MeetupDuration(
                hours: 3,
                minutes: 30));
        context.Meetups.Add(meetup);

        var organizer = new Organizer(
            username: "organizer",
            password: BCrypt.HashPassword("pa$$word"),
            displayName: "Some organizer");
        organizer.AddOrganizedMeetup(meetup);
        context.Organizers.Add(organizer);

        await context.SaveChangesAsync();
        
        // Act

        var request = new Request(
            MeetupId: meetup.Id,
            Place: new MeetupPlaceModel(
                CityId: Guid.NewGuid(),
                Address: "Some new address"),
            StartTime: DateTime.UtcNow.AddDays(3),
            Duration: new MeetupDurationModel(
                Hours: 2,
                Minutes: 45),
            CurrentUserId: organizer.Id);
        var response = await requestHandler.HandleRequest(request);
        
        // Assert
        
        Assert.False(response.Success);
        Assert.Equal(ErrorTypes.CityDoesNotExist, response.ErrorType);
    }
}