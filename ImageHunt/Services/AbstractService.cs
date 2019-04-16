using ImageHunt.Data;
using Microsoft.Extensions.Logging;

namespace ImageHuntCore.Services
{
    public abstract class AbstractService : IService
    {
      protected ILogger Logger;
      protected HuntContext Context { get; }

        public AbstractService(HuntContext context, ILogger logger)
        {
            Context = context;
          Logger = logger;
        }

  }
}
