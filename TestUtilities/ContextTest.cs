using System;
using System.Collections.Generic;
using System.Text;
using Autofac.Extras.FakeItEasy;
using Microsoft.EntityFrameworkCore;

namespace TestUtilities
{
    public class ContextTest<TARGET, CONTEXT> : IDisposable where TARGET : class
                                              where CONTEXT : DbContext
    {
        protected AutoFake _fake;
        protected TARGET _target;
        protected CONTEXT _context;

        public ContextTest()
        {
            _fake = new AutoFake();
        }

        public TARGET ResolveTarget()
        {
            _target = _fake.Resolve<TARGET>();
            _context = _fake.Resolve<CONTEXT>();
            return _target;
        }
        public void Dispose()
        {
            _fake?.Dispose();
        }
    }
}
