using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;

namespace ImageHuntBotBuilder
{
    public interface IStatePropertyAccessorExtended<T> 
    {
        Task<IEnumerable<T>> GetAllAsync();
    }
}
