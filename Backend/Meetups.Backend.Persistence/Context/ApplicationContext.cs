namespace Meetups.Backend.Persistence.Context;

using Meetups.Backend.Entities.Meetup;
using Meetups.Backend.Entities.User;
using Microsoft.EntityFrameworkCore;

public class ApplicationContext : DbContext
{
    public DbSet<City> Cities => Set<City>();
    public DbSet<Meetup> Meetups => Set<Meetup>();
    
    public DbSet<Guest> Guests => Set<Guest>();
    public DbSet<Organizer> Organizers => Set<Organizer>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<User> Users => Set<User>();

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder
            .ApplyEntityTypeConfigurations()
            .ApplyRelationshipConfigurations();
}
