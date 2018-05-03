using System;
using System.Collections.Generic;
using System.Text;
using ImageHunt;
using Xunit;

namespace ImageHuntTest
{
  public class AutomapperFixture : IDisposable
  {
    public AutomapperFixture()
    {
      Startup.ConfigureMappings();
    }
    public void Dispose()
    {
    }
  }
  [CollectionDefinition("AutomapperFixture")]
  public class TestCollectionFixture : ICollectionFixture<AutomapperFixture>
    {
    }
}
