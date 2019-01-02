using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrleansIndexingGrainInterfaces
{
    public class GrainIndexManager_NonUnique<TIndexKey, TIGrain> : GrainIndexManagerBase<TIndexKey, TIGrain, IIndexerGrain_NonUnique<TIGrain, TIndexKey>>
        where TIGrain : class, IGrain
    {
        public GrainIndexManager_NonUnique(string KeyName, int NumIndexerGrains, IKeyHashGenerator<TIndexKey> HashGenerator) : base(KeyName, NumIndexerGrains, HashGenerator) { }

        public async Task<IEnumerable<TIGrain>> GetAllGrains(IGrainFactory GrainFactory, TIndexKey Key) => await GetIndexer(GrainFactory, Key).GetGrains(Key);

        public async Task RemoveAll(IGrainFactory GrainFactory, TIndexKey Key) => await GetIndexer(GrainFactory, Key).RemoveAll(Key);

        public async Task RemoveOne(IGrainFactory GrainFactory, TIndexKey Key, TIGrain Grain) => await GetIndexer(GrainFactory, Key).RemoveOne(Key, Grain.AsReference<TIGrain>());

        public async Task AddToIndex(IGrainFactory GrainFactory, TIndexKey Key, TIGrain Grain) => await GetIndexer(GrainFactory, Key).AddIndex(Key, Grain.AsReference<TIGrain>());

    }
}
