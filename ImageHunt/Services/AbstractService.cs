using ImageHunt.Data;

namespace ImageHunt.Services
{
    public abstract class AbstractService : IService
    {
        public HuntContext Context { get; }

        public AbstractService(HuntContext context)
        {
            Context = context;
        }
    }
}