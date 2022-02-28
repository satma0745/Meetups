namespace Meetups.Tests.Features.Studio;

using System;
using System.Threading.Tasks;
using BCrypt.Net;
using Meetups.Application.Features.Studio.RegisterNewMeetup.Internal;
using Meetups.Application.Modules.Persistence;
using Meetups.Domain.Entities.Meetup;
using Meetups.Domain.Entities.User;
using Meetups.Tests.Seedwork;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class RegisterNewMeetupFeatureTests
{
    private readonly IApplicationContext context;
    private readonly RequestHandler requestHandler;

    public RegisterNewMeetupFeatureTests()
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

        var organizer = new Organizer(
            username: "Some organizer",
            password: BCrypt.HashPassword("pa$$word"),
            displayName: "Some organizer");
        context.Organizers.Add(organizer);
        
        await context.SaveChangesAsync();
        
        // Act

        var request = new Request(
            Topic: "Some meetup",
            Place: new MeetupPlaceModel(
                CityId: oslo.Id,
                Address: "Some address"),
            StartTime: DateTime.UtcNow,
            Duration: new MeetupDurationModel(
                Hours: 3,
                Minutes: 30),
            OrganizerId: organizer.Id);
        var response = await requestHandler.HandleRequest(request);
        
        // Assert
        
        Assert.True(response.Success);

        var persisted = await context.Meetups.SingleOrDefaultAsync();
        Assert.NotNull(persisted);
        
        Assert.Equal(request.Topic, persisted!.Topic);
        Assert.Equal(request.Place.CityId, persisted.Place.City.Id);
        Assert.Equal(request.Place.Address, persisted.Place.Address);
        Assert.Equal(request.StartTime, persisted.StartTime);
        Assert.Equal(request.Duration.Hours, persisted.Duration.Hours);
        Assert.Equal(request.Duration.Minutes, persisted.Duration.Minutes);
        Assert.Equal(request.OrganizerId, persisted.Organizer.Id);
    }

    [Fact]
    public async Task FailureForAlreadyTakenTopic()
    {
        // Arrange

        var oslo = new City("Oslo");
        context.Cities.Add(oslo);

        var otherMeetup = new Meetup(
            topic: "Taken topic",
            place: new MeetupPlace(
                city: oslo,
                address: "Some address"),
            duration: new MeetupDuration(
                hours: 1,
                minutes: 30),
            startTime: DateTime.UtcNow);
        context.Meetups.Add(otherMeetup);
        
        var organizer = new Organizer(
            username: "Some organizer",
            password: BCrypt.HashPassword("pa$$word"),
            displayName: "Some organizer");
        organizer.AddOrganizedMeetup(otherMeetup);
        context.Organizers.Add(organizer);
        
        await context.SaveChangesAsync();
        
        // Act

        var request = new Request(
            Topic: "Taken topic",
            Place: new MeetupPlaceModel(
                CityId: oslo.Id,
                Address: "Some address"),
            StartTime: DateTime.UtcNow,
            Duration: new MeetupDurationModel(
                Hours: 3,
                Minutes: 30),
            OrganizerId: organizer.Id);
        var response = await requestHandler.HandleRequest(request);
        
        // Assert
        
        Assert.False(response.Success);
        Assert.Equal(ErrorTypes.TopicAlreadyTaken, response.ErrorType);
    }

    [Fact]
    public async Task FailureForNotExistingCity()
    {
        // Arrange

        var organizer = new Organizer(
            username: "Some organizer",
            password: BCrypt.HashPassword("pa$$word"),
            displayName: "Some organizer");
        context.Organizers.Add(organizer);
        await context.SaveChangesAsync();
        
        // Act

        var request = new Request(
            Topic: "Taken topic",
            Place: new MeetupPlaceModel(
                CityId: Guid.NewGuid(),
                Address: "Some address"),
            StartTime: DateTime.UtcNow,
            Duration: new MeetupDurationModel(
                Hours: 3,
                Minutes: 30),
            OrganizerId: organizer.Id);
        var response = await requestHandler.HandleRequest(request);
        
        // Assert
        
        Assert.False(response.Success);
        Assert.Equal(ErrorTypes.CityDoesNotExist, response.ErrorType);
    }
}