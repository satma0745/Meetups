namespace Meetups.Context;

using Meetups.Entities;
using Microsoft.EntityFrameworkCore;

public class ApplicationContext : DbContext
{
    public DbSet<Meetup> Meetups { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder contextOptions)
    {
        // TODO: retrieve from the environment
        var host = "localhost";
        var port = "5432";
        var database = "meetups";
        var username = "postgres";
        var password = "none_of_your_business";
        
        var connectionString = $"Server={host};Port={port};Database={database};User Id={username};Password={password};";
        contextOptions.UseNpgsql(connectionString);
    }
}
