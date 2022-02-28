namespace Meetups.Tests.Seedwork;

using System;
using Meetups.Application.Modules.Persistence;
using Meetups.Application.Modules.Persistence.Context;
using Microsoft.EntityFrameworkCore;

internal static class ContextFactory
{
    public static IApplicationContext InMemory()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

        var options = optionsBuilder.Options;
        var context = new ApplicationContext(options);

        return context;
    }
}