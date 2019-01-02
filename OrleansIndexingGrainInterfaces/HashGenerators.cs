using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrleansIndexingGrainInterfaces
{
    public interface IKeyHashGenerator<TIndexKey>
    {
        uint GenerateHash(TIndexKey Key);
    }


    public class StringHashGenerator : IKeyHashGenerator<string>
    {
        public uint GenerateHash(string Key)
        {
            return JenkinsHash.ComputeHash(Key);
        }
    }

    public class IntHashGenerator : IKeyHashGenerator<int>
    {
        public uint GenerateHash(int Key)
        {
            return unchecked((uint)Key);
        }
    }

    public class UIntHashGenerator : IKeyHashGenerator<uint>
    {
        public uint GenerateHash(uint Key)
        {
            return Key;
        }
    }

    public class GuidHashGenerator : IKeyHashGenerator<Guid>
    {
        public uint GenerateHash(Guid Key)
        {
            return JenkinsHash.ComputeHash(Key.ToByteArray());
        }
    }

    public class LongHashGenerator : IKeyHashGenerator<long>
    {
        public uint GenerateHash(long Key)
        {
            return JenkinsHash.ComputeHash(BitConverter.GetBytes(Key));
        }
    }

    public class ULongHashGenerator : IKeyHashGenerator<ulong>
    {
        public uint GenerateHash(ulong Key)
        {
            return JenkinsHash.ComputeHash(BitConverter.GetBytes(Key));
        }
    }
}
