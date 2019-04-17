using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;

namespace ImageHuntBotBuilder
{
    public class MultiConversationState<T> : IStatePropertyAccessorExtended<T> where T : class
    {
        private readonly IMultiStorage _storage;

        public MultiConversationState(IMultiStorage storage)
        {
            _storage = storage;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var data = await _storage.ReadAllAsync();
            var list = new List<T>();
            foreach (var d in data)
            {
                var sub = d.Values.Select(v=>(IDictionary<string, object>)v).ToList();
                var dic = new List<T>();
                foreach (var s in sub)
                {
                    var vals = s.Values.Select(v => v as T);
                    list.AddRange(vals.Where(state => state != null));
                }
            }

            return list;
        }

        public string Name { get; }

        public Task<T> GetAsync(
            ITurnContext turnContext, 
            Func<T> defaultValueFactory = null,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(ITurnContext turnContext, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task SetAsync(ITurnContext turnContext, T value, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }
    }
}