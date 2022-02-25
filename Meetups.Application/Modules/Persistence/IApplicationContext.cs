namespace Meetups.Application.Modules.Persistence;

using System.Threading.Tasks;
using Meetups.Domain.Entities.Meetup;
using Meetups.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;

public interface IApplicationContext
{
    DbSet<City> Cities { get; }
    DbSet<Meetup> Meetups { get; }
    
    DbSet<Guest> Guests { get; }
    DbSet<Organizer> Organizers { get; }
    DbSet<User> Users { get; }

    Task SaveChangesAsync();
}