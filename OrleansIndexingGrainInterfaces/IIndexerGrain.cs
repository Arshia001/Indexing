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
    public interface IIndexerGrain : IGrainWithIntegerCompoundKey { }
}
