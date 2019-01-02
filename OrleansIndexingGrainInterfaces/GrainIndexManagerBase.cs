using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrleansIndexingGrainInterfaces
{
#if DEBUG
    static class GrainIndexKeyNameChecker
    {
        static HashSet<string> UsedNames = new HashSet<string>();

        public static bool CheckAndAdd(string Name)
        {
            lock (UsedNames)
            {
                if (UsedNames.Contains(Name))
                    return false;

                UsedNames.Add(Name);
                return true;
            }
        }
    }
#endif


    public abstract class GrainIndexManagerBase<TIndexKey, TIGrain, TIIndexer>
        where TIGrain : IGrain
        where TIIndexer : IIndexerGrain
    {
        protected string keyName;
        protected int numIndexerGrains;
        protected IKeyHashGenerator<TIndexKey> hashGenerator;


        /// <summary>
        /// NOTE: key name must be unique across entire project, including across types.
        /// </summary>
        protected GrainIndexManagerBase(string KeyName, int NumIndexerGrains, IKeyHashGenerator<TIndexKey> HashGenerator)
        {
#if DEBUG
            // This, of course, assumes index managers are singletons, which they should be anyway
            if (!GrainIndexKeyNameChecker.CheckAndAdd(KeyName))
                throw new ArgumentException("Cannot use duplicate key names for indexing", nameof(KeyName));
#endif

            this.keyName = KeyName;
            this.numIndexerGrains = NumIndexerGrains;
            this.hashGenerator = HashGenerator;
        }

        protected TIIndexer GetIndexer(IGrainFactory GrainFactory, TIndexKey Key)
        {
            var KeyHash = (uint)(hashGenerator.GenerateHash(Key) % numIndexerGrains);
            return GrainFactory.GetGrain<TIIndexer>(KeyHash, keyName, null);
        }
    }
}
