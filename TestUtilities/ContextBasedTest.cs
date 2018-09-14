using System;
using ImageHunt.Data;
using ImageHuntCore;
using Microsoft.EntityFrameworkCore;

namespace TestUtilities
{
    public class ContextBasedTest<CONTEXT> : BaseTest, IDisposable
        where CONTEXT : DbContext
    {
        protected CONTEXT _context;

        public ContextBasedTest()
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<CONTEXT>()
                .UseSqlite("DataSource=:memory:") 
                .EnableSensitiveDataLogging();
            _context = ActivableContext<CONTEXT>.CreateInstance(dbContextOptionsBuilder.Options);
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