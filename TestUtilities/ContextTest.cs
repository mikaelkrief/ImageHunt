using System;
using Autofac.Extras.FakeItEasy;
using Microsoft.EntityFrameworkCore;

namespace TestUtilities
{
    public class ContextTest<TArget, TContext> : IDisposable where TArget : class
                                              where TContext : DbContext
    {
        protected AutoFake Fake;
        protected TArget Target;
        protected TContext Context;

        public ContextTest()
        {
            Fake = new AutoFake();
        }

        public TArget ResolveTarget()
        {
            Target = Fake.Resolve<TArget>();
            Context = Fake.Resolve<TContext>();
            return Target;
        }
        public void Dispose()
        {
            Fake?.Dispose();
        }
    }
}
