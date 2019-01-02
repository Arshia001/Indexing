using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrleansIndexingGrainInterfaces
{
    public class GrainIndexManager_Unique<TIndexKey, TIGrain> : GrainIndexManagerBase<TIndexKey, TIGrain, IIndexerGrain_Unique<TIGrain, TIndexKey>>
        where TIGrain : class, IGrain
    {
        public GrainIndexManager_Unique(string KeyName, int NumIndexerGrains, IKeyHashGenerator<TIndexKey> HashGenerator) : base(KeyName, NumIndexerGrains, HashGenerator) { }

        public async Task<TIGrain> GetGrain(IGrainFactory GrainFactory, TIndexKey Key) => await GetIndexer(GrainFactory, Key).GetGrain(Key);

        public async Task RemoveIndex(IGrainFactory GrainFactory, TIndexKey Key) => await GetIndexer(GrainFactory, Key).RemoveKey(Key);

        public async Task<bool> RenameIndex(IGrainFactory GrainFactory, TIndexKey OldKey, TIndexKey NewKey)
        {
            var Grain = await GetGrain(GrainFactory, OldKey);
            var GrainAtNewKey = await GetGrain(GrainFactory, NewKey);

            if (Grain != null && GrainAtNewKey == null)
            {
                await RemoveIndex(GrainFactory, OldKey);
                await UpdateIndex(GrainFactory, NewKey, Grain);
                return true;
            }

            return false;
        }

        public async Task UpdateIndex(IGrainFactory GrainFactory, TIndexKey Key, TIGrain Grain) => await GetIndexer(GrainFactory, Key).UpdateIndex(Key, Grain.AsReference<TIGrain>());

        public async Task<bool> UpdateIndexIfUnique(IGrainFactory GrainFactory, TIndexKey Key, TIGrain Grain) => await GetIndexer(GrainFactory, Key).UpdateIndexIfUnique(Key, Grain.AsReference<TIGrain>());
    }
}
