using MagureanuStefan_API.DataContext;
using MagureanuStefan_API.Models;
using Microsoft.EntityFrameworkCore;

namespace MagureanuStefan_MVC.UnitTests.Helpers
{
    public class DbContextHelper
    {
        public static ClubLibraDataContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<ClubLibraDataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .Options;
            //UseInMemoryDatabase -> permite configurarea si utilizarea unei baze de date in memorie

            var databaseContext = new ClubLibraDataContext(options);
            databaseContext.Database.EnsureCreated();
            return databaseContext;
        }
        public static Announcement AddAnnouncement(ClubLibraDataContext dbContext, Announcement model)
        {
            dbContext.Add(model);
            dbContext.SaveChanges();
            dbContext.Entry(model).State = EntityState.Detached;
            return model;
        }
    }
}
