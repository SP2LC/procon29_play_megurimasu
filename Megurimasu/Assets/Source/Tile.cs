using System;
using UnityEngine;

namespace Meguru
{
    [Serializable]
    public class Tile
    {
        public int stat;
        public int point;

        public Tile(int stat, int point)
        {
            this.stat = stat;
            this.point = point;
        }
    }
}