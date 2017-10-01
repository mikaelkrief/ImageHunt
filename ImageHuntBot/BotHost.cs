using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageHuntBot
{
    public class BotHost
    {
      public static IBotHostBuilder CreateDefaultBuilder(string[] args)
      {
        return new BotHostBuilder();
      }
    }

  public class BotHostBuilder : IBotHostBuilder
  {
    public IBotHost Build()
    {
      throw new NotImplementedException();
    }
  }

  public interface IBotHostBuilder
  {
    IBotHost Build();
  }

  public interface IBotHost
  {
    void Start();
  }
}
