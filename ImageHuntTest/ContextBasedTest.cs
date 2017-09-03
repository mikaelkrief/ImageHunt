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
            var dbContextOptionsBuilder = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Unique database name for each test
                .EnableSensitiveDataLogging();
            _context = new HuntContext(dbContextOptionsBuilder.Options);
            Console.WriteLine($"{_context}");
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}