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
    public interface IIndexerGrain_NonUnique<TIGrain, TKey> : IIndexerGrain
        where TIGrain : IGrain
    {
        Task<IEnumerable<TIGrain>> GetGrains(TKey Key);
        Task RemoveOne(TKey Key, TIGrain Grain);
        Task RemoveAll(TKey Key);
        Task AddIndex(TKey Key, TIGrain Grain);
    }
}
