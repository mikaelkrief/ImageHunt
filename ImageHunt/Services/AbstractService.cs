using ImageHunt.Data;
using Microsoft.Extensions.Logging;

namespace ImageHuntCore.Services
{
  public abstract class AbstractService : IService
  {
    protected ILogger _logger;

    public AbstractService(HuntContext context, ILogger logger)
    {
      Context = context;
      _logger = logger;
    }

    protected HuntContext Context { get; }
  }
}
