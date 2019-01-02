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
    public class IndexerGrainNonUniqueState<TIGrain, TKey> : IGenericSerializable
        where TIGrain : IGrain
    {
        public IndexerGrainNonUniqueState()
        {
            Data = new Dictionary<TKey, HashSet<TIGrain>>();
        }

        public Dictionary<TKey, HashSet<TIGrain>> Data { get; set; }
    }

    public class IndexerGrainNonUnique<TIGrain, TKey> : Grain<IndexerGrainNonUniqueState<TIGrain, TKey>>, IIndexerGrain_NonUnique<TIGrain, TKey>
        where TIGrain : IGrain
    {
        public Task<IEnumerable<TIGrain>> GetGrains(TKey Key) => Task.FromResult(State.Data.TryGetValue(Key, out var Result) ? Result.AsEnumerable() : null);

        public Task RemoveOne(TKey Key, TIGrain Grain)
        {
            if (State.Data.TryGetValue(Key, out var Result))
            {
                Result.Remove(Grain);
                if (Result.Count == 0)
                    State.Data.Remove(Key);
                return WriteStateAsync();
            }

            return Task.CompletedTask;
        }

        public Task RemoveAll(TKey Key)
        {
            State.Data.Remove(Key);
            return WriteStateAsync();
        }

        public Task AddIndex(TKey Key, TIGrain Grain)
        {
            if (!State.Data.TryGetValue(Key, out var Set))
                State.Data[Key] = Set = new HashSet<TIGrain>();

            // A Contains call is O(1), WriteStateAsync is O(n) and interacts with storage
            if (!Set.Contains(Grain))
            {
                Set.Add(Grain);
                return WriteStateAsync();
            }

            return Task.CompletedTask;
        }
    }
}
