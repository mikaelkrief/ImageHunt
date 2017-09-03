using System;
using ImageHunt.Data;
using Microsoft.EntityFrameworkCore;

namespace ImageHuntTest
{
    public class ContextBasedTest : IDisposable
    {
        protected HuntContext _context;

        public ContextBasedTest()
        {
            //var dbContextOptionsBuilder = new DbContextOptionsBuilder()
            //    .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Unique database name for each test
            //    .EnableSensitiveDataLogging();
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<HuntContext>()
                .UseSqlite("DataSource=:memory:") 
                .EnableSensitiveDataLogging();
            //using (var context = new HuntContext(dbContextOptionsBuilder.Options))
            //{
            //    context.Database.OpenConnection();
            //    context.Database.EnsureCreated();
            //    context.Database.ExecuteSqlCommand("alter table Nodes add Coordinate point null;");
            //}
            _context = new HuntContext(dbContextOptionsBuilder.Options);
            _context.Database.OpenConnection();
            _context.Database.EnsureCreated();
            _context.Database.ExecuteSqlCommand("alter table Nodes add Coordinate point null;");
            Console.WriteLine($"{_context}");
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}