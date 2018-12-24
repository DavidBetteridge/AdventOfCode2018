using System;
using System.Collections.Generic;

namespace Day23
{
    class Region
    {
        public int X1 { get; set; }
        public int X2 { get; set; }
        public int Y1 { get; set; }
        public int Y2 { get; set; }
        public int Z1 { get; set; }
        public int Z2 { get; set; }
        public int Score { get; set; }

        internal bool IsInSide(Region region)
        {
            return (X1 >= region.X1 &&
                    X2 <= region.X2 &&
                    Y1 >= region.Y1 &&
                    Y2 <= region.Y2 &&
                    Z1 >= region.Z1 &&
                    Z2 >= region.Z2);
        }

        public override string ToString()
        {
            return $"X: {X1,8}->{X2,8}   Y:{Y1,8}->{Y2,8}   Z:{Z1,8}->{Z2,8}";
        }

        internal bool CutsInto(Region region)
        {
            var p1 = (X1 >= region.X1 && X1 <= region.X2 && 
                      Y1 >= region.Y1 && Y1 <= region.Y2 &&
                      Z1 >= region.Z1 && Z1 <= region.Z2);

            var p2 = (X2 >= region.X1 && X2 <= region.X2 &&
                      Y2 >= region.Y1 && Y2 <= region.Y2 &&
                      Z2 >= region.Z1 && Z2 <= region.Z2);

            return (p1 || p2);

        }

        //internal IEnumerable<Region> Split(Region region)
        //{
        //    // This region has hit into the other region

        //    // One Face only
        //    //       Break this region into 2
        //    //       Break region into 5

        //    // Two faces (Edge)
        //    //      Break this region into 3
        //    //      Break region into 5

        //    // Three faces (Corner)
        //    //      Break this region into 4
        //    //      Break region into 4


        //}
    }
}
