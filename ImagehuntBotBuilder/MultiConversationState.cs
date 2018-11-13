﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Newtonsoft.Json.Linq;

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
                    var vals = s.Values.Select(v=>(T)v);
                    list.AddRange(vals);
                }
            }

            return list;
        }
    }
}