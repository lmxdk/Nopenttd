using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nopenttd.Core;

namespace Nopenttd.Core
{
    /** Stores the state of all random number generators */
    struct SavedRandomSeeds
    {
        public Randomizer random;
        public Randomizer interactive_random;
    };

}
