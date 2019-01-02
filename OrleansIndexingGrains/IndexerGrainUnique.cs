using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;
using Orleans.Providers;
using Bond;
using OrleansBondUtils;
using OrleansIndexingGrainInterfaces;

namespace OrleansIndexingGrains
{
    public class IndexerGrainUniqueState<TIGrain, TKey> : IGenericSerializable
        where TIGrain : IGrain
    {
        public IndexerGrainUniqueState()
        {
            Data = new Dictionary<TKey, TIGrain>();
        }

        public Dictionary<TKey, TIGrain> Data { get; set; }
    }

    public class IndexerGrainUnique<TIGrain, TKey> : Grain<IndexerGrainUniqueState<TIGrain, TKey>>, IIndexerGrain_Unique<TIGrain, TKey>
        where TIGrain : IGrain
    {
        public Task<TIGrain> GetGrain(TKey Key) => Task.FromResult(State.Data.TryGetValue(Key, out var Result) ? Result : default(TIGrain));

        public async Task RemoveKey(TKey Key)
        {
            State.Data.Remove(Key);
            await WriteStateAsync();
        }

        public async Task UpdateIndex(TKey Key, TIGrain Grain)
        {
            State.Data[Key] = Grain;
            await WriteStateAsync();
        }

        public async Task<bool> UpdateIndexIfUnique(TKey Key, TIGrain Grain)
        {
            if (!State.Data.ContainsKey(Key))
            {
                State.Data[Key] = Grain;
                await WriteStateAsync();
                return true;
            }

            return State.Data[Key].Equals(Grain);
        }
    }
}
