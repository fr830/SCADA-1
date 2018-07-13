using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADA
{
    class Work_Simulation
    {
        private static readonly Lazy<Work_Simulation> lazy = new Lazy<Work_Simulation>(() => new Work_Simulation());

        public static Work_Simulation Instance { get { return lazy.Value; } }
    }
}
