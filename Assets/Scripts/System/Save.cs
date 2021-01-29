using LonelyIsland.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LonelyIsland.System
{
    public class Save
    {
        public int Coins { get; set; }
        public int Level { get; set; }

        public SerializedVector3 Position { get; set; }
    }
}
