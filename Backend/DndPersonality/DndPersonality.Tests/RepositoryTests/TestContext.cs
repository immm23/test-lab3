using DndPersonality.DAL;
using Microsoft.EntityFrameworkCore;

namespace DndPersonality.Tests.RepositoryTests
{
    public class TestContext
    {
        public static Context CreateContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .EnableSensitiveDataLogging()
            .Options;

            var context = new Context(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return context;
        }
    }
}
