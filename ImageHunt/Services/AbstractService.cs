using ImageHunt.Data;
using ImageHunt.Services;
using Microsoft.Extensions.Logging;

namespace ImageHuntCore.Services
{
    public abstract class AbstractService : IService
    {
      protected ILogger _logger;
      protected HuntContext Context { get; }

        public AbstractService(HuntContext context, ILogger logger)
        {
            Context = context;
          _logger = logger;
        }

  }
}
