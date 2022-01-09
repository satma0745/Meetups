namespace Meetups.Context;

using Meetups.Entities;
using Microsoft.EntityFrameworkCore;

public class Context : DbContext
{
    public DbSet<Meetup> Meetups { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseInMemoryDatabase("Meetups in-memory database.");
}
