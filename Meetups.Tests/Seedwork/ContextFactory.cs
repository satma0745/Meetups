namespace Meetups.Tests.Seedwork;

using Meetups.Application.Modules.Persistence;
using Meetups.Application.Modules.Persistence.Context;
using Microsoft.EntityFrameworkCore;

internal static class ContextFactory
{
    public static IApplicationContext InMemory()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
        optionsBuilder.UseInMemoryDatabase("In-memory DB");

        var options = optionsBuilder.Options;
        var context = new ApplicationContext(options);

        return context;
    }
}