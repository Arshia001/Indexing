#if !NO_BOND
using OrleansBondUtils;
#endif
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrleansIndexingGrainInterfaces
{
    public interface IIndexerGrain_Unique<TIGrain, TKey> : IIndexerGrain
        where TIGrain : IGrain
    {
        Task<TIGrain> GetGrain(TKey Key);
        Task RemoveKey(TKey Key);
        Task UpdateIndex(TKey Key, TIGrain Grain);
        Task<bool> UpdateIndexIfUnique(TKey Key, TIGrain Grain);
    }
}
