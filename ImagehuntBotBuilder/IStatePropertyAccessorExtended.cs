using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageHuntBotBuilder
{
    public interface IStatePropertyAccessorExtended<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
    }
}