using System;
using ImageHuntCore;
using Microsoft.EntityFrameworkCore;

namespace TestUtilities
{
    public class ContextBasedTest<TContext> : BaseTest, IDisposable
        where TContext : DbContext
    {
        protected TContext Context;

        public ContextBasedTest()
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<TContext>()
                .UseSqlite("DataSource=:memory:") 
                .EnableSensitiveDataLogging();
            Context = ActivableContext<TContext>.CreateInstance(dbContextOptionsBuilder.Options);
            Context.Database.OpenConnection();
            Context.Database.EnsureCreated();
            Context.Database.ExecuteSqlCommand("alter table Nodes add Coordinate point null;");
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}