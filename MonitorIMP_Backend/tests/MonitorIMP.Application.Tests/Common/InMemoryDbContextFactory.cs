using Microsoft.EntityFrameworkCore;
using MonitorIMP.Infrastructure.Persistence;

namespace MonitorIMP.Application.Tests.Common;

internal static class InMemoryDbContextFactory
{
    public static ApplicationDbContext Create(string? databaseName = null)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName ?? Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }
}