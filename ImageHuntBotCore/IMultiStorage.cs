using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;

namespace ImageHuntBotCore
{
    public interface IMultiStorage  : IStorage
    {
        Task<IEnumerable<IDictionary<string, object>>> ReadAllAsync(
            CancellationToken cancellationToken = new CancellationToken());
    }
}