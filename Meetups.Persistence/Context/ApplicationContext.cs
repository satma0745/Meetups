namespace Meetups.Persistence.Context;

using System.Threading.Tasks;
using Meetups.Application.Modules.Persistence;
using Meetups.Domain.Entities.Meetup;
using Meetups.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;

public class ApplicationContext : DbContext, IApplicationContext
{
    public DbSet<City> Cities => Set<City>();
    public DbSet<Meetup> Meetups => Set<Meetup>();
    
    public DbSet<Guest> Guests => Set<Guest>();
    public DbSet<Organizer> Organizers => Set<Organizer>();
    public DbSet<User> Users => Set<User>();

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder
            .ApplyEntityTypeConfigurations()
            .ApplyRelationshipConfigurations();

    public async Task SaveChangesAsync() =>
        await base.SaveChangesAsync();
}
