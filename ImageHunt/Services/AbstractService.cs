using ImageHunt.Data;

namespace ImageHuntCore.Services
{
    public abstract class AbstractService : IService
    {
        protected HuntContext Context { get; }

        public AbstractService(HuntContext context)
        {
            Context = context;
        }
    }
}
