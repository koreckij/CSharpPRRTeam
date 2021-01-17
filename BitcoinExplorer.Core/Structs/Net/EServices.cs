using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitcoinExplorer.Core.Structs.Net
{
    [Flags]
    public enum Services : ulong
    {
        NODE_NETWORK = 1,
        NODE_GETUTXO = 2,
        NODE_BLOOM = 4,
        NODE_WITNESS = 8,
        NODE_XTHIN = 16,
        NODE_COMPACT_FILTERS = 64,
        NODE_NETWORK_LIMITED = 1024
    }
}
