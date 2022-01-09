namespace Meetups.Context;

using Meetups.Entities;
using Microsoft.EntityFrameworkCore;

public class MeetupsContext : DbContext
{
    public DbSet<Meetup> Meetups { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseInMemoryDatabase("Meetups in-memory database.");
}
