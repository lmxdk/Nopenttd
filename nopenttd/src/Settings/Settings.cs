using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nopenttd.src.Settings
{
    class Settings
    {
    }

    //mock
    public class _settings_game
    {
        public static ConstructionSettings construction { get; } = new ConstructionSettings();
    }

    public class ConstructionSettings
    {
        public bool freeform_edges = false;
    }
}
