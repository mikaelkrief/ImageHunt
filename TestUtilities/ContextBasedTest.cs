using System;
using ImageHunt.Data;
using Microsoft.EntityFrameworkCore;

namespace TestUtilities
{
    public class ContextBasedTest : BaseTest, IDisposable
    {
        protected HuntContext _context;

        public ContextBasedTest()
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<HuntContext>()
                .UseSqlite("DataSource=:memory:") 
                .EnableSensitiveDataLogging();
            _context = new HuntContext(dbContextOptionsBuilder.Options);
            _context.Database.OpenConnection();
            _context.Database.EnsureCreated();
            _context.Database.ExecuteSqlCommand("alter table Nodes add Coordinate point null;");
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}