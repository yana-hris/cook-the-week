namespace CookTheWeek.Services.Tests.UnitTests
{
    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data;

    public static class DatabaseMock
    {
        public static CookTheWeekDbContext Instance
        {
            get
            {
                var dbContextOptions = new DbContextOptionsBuilder<CookTheWeekDbContext>()
                    .UseInMemoryDatabase("CookTheWeekInMemoryDb" + DateTime.Now.Ticks.ToString())
                    .Options;

                return new CookTheWeekDbContext(dbContextOptions, false);
            }
        }
    }
}
