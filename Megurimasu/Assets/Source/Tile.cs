using System;
using UnityEngine;

namespace Meguru
{
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