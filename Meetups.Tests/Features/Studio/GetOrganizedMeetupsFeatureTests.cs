namespace Meetups.Tests.Features.Studio;

using System;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;
using Meetups.Application.Features.Studio.GetOrganizedMeetups.Internal;
using Meetups.Application.Modules.Persistence;
using Meetups.Domain.Entities.Meetup;
using Meetups.Domain.Entities.User;
using Meetups.Tests.Seedwork;
using Xunit;

public class GetOrganizedMeetupsFeatureTests
{
    private readonly IApplicationContext context;
    private readonly RequestHandler requestHandler;

    public GetOrganizedMeetupsFeatureTests()
    {
        context = ContextFactory.InMemory();
        requestHandler = new RequestHandler(context);
    }

    [Fact]
    public async Task Success()
    {
        // Arrange

        var targetOrganizer = new Organizer(
            username: "targetOrganizer",
            password: BCrypt.HashPassword("pa$$word"),
            displayName: "Meetup organizer #1");
        var otherOrganizer = new Organizer(
            username: "otherOrganizer",
            password: BCrypt.HashPassword("pa$$word"),
            displayName: "Meetup organizer #2");
        context.Users.AddRange(targetOrganizer, otherOrganizer);
        
        var oslo = new City("Oslo");
        var austin = new City("Austin");
        context.Cities.AddRange(oslo, austin);

        var targetMeetups = new[]
        {
            new Meetup(
                topic: "Meetup #1",
                place: new MeetupPlace(
                    city: oslo,
                    address: "Meetup #1: Address"),
                startTime: DateTime.UtcNow,
                duration: new MeetupDuration(
                    hours: 1,
                    minutes: 15)),
            new Meetup(
                topic: "Meetup #2",
                place: new MeetupPlace(
                    city: oslo,
                    address: "Meetup #2: Address"),
                startTime: DateTime.UtcNow,
                duration: new MeetupDuration(
                    hours: 2,
                    minutes: 30)),
            new Meetup(
                topic: "Meetup #3",
                place: new MeetupPlace(
                    city: austin,
                    address: "Meetup #3: Address"),
                startTime: DateTime.UtcNow,
                duration: new MeetupDuration(
                    hours: 3,
                    minutes: 45))
        };
        var otherMeetup = new Meetup(
            topic: "Meetup #4",
            place: new MeetupPlace(
                city: austin,
                address: "Meetup #4: Address"),
            startTime: DateTime.UtcNow,
            duration: new MeetupDuration(
                hours: 4,
                minutes: 00));
        context.Meetups.AddRange(targetMeetups);
        context.Meetups.Add(otherMeetup);

        foreach (var meetup in targetMeetups)
        {
            targetOrganizer.AddOrganizedMeetup(meetup);
        }
        await context.SaveChangesAsync();
        
        // Act

        var request = new Request(targetOrganizer.Id);
        var response = await requestHandler.HandleRequest(request);
        
        // Assert
        
        Assert.True(response.Success);
        Assert.NotNull(response.Payload);
        Assert.Equal(targetMeetups.Length, response.Payload.Count);

        var pairs = targetMeetups
            .Join(
                response.Payload,
                meetup => meetup.Id,
                result => result.Id,
                (meetup, result) => (meetup, result))
            .ToList();
        Assert.Equal(targetMeetups.Length, pairs.Count);
        foreach (var (meetup, result) in pairs)
        {
            Assert.Equal(meetup.Topic, result.Topic);
            Assert.Equal(meetup.Place.City.Id, result.Place.CityId);
            Assert.Equal(meetup.Place.City.Name, result.Place.CityName);
            Assert.Equal(meetup.Place.Address, result.Place.Address);
            Assert.Equal(meetup.StartTime, result.StartTime);
            Assert.Equal(meetup.Duration.Hours, result.Duration.Hours);
            Assert.Equal(meetup.Duration.Minutes, result.Duration.Minutes);
        }
    }
}