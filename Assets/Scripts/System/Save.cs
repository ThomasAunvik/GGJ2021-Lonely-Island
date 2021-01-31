using LonelyIsland.Characters;
using LonelyIsland.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LonelyIsland.System
{
    public class Save
    {
        public int Coins { get; set; } = 0;
        public int Level { get; set; } = 1;
        public float Health { get; set; } = -1;
        public Stats Stats { get; set; } = new Stats();
        public List<int> PurchasedItems = new List<int>();

        public SerializedVector3 Position { get; set; }
    }
}
