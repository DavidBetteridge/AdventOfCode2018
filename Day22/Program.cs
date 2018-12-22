using System;

namespace Day22
{
    class Program
    {
        public enum RegionType
        {
            Rocky = 0,
            Wet = 1,
            Narrow = 2,
        }

        private static readonly int targetX = 6;
        private static readonly int targetY = 770;
        private static readonly int depth = 4845;

        public static int[,] erosionLevels { get; private set; }

        static void Main(string[] args)
        {
            var map = new RegionType[targetX + 1, targetY + 1];
            erosionLevels = new int[targetX+1, targetY + 1];


            for (int x = 0; x <= map.GetUpperBound(0); x++)
            {
                for (int y = 0; y <= map.GetUpperBound(1); y++)
                {
                    erosionLevels[x, y] = -1;
                }
            }


            for (int x = 0; x <= map.GetUpperBound(0); x++)
            {
                for (int y = 0; y <= map.GetUpperBound(1); y++)
                {
                    map[x, y] = FindRegionType(x, y);
                }
            }

            //for (int y = 0; y <= map.GetUpperBound(1); y++)
            //{
            //    var line = "";
            //    for (int x = 0; x <= map.GetUpperBound(0); x++)
            //    {
            //        if (x == 0 && y == 0)
            //            line += 'M';
            //        else if (x == targetX && y == targetY)
            //            line += 'T';
            //        else
            //            switch (map[x,y])
            //            {
            //                case RegionType.Rocky:
            //                    line += '.';
            //                    break;
            //                case RegionType.Wet:
            //                    line += '=';
            //                    break;
            //                case RegionType.Narrow:
            //                    line += '|';
            //                    break;
            //                default:
            //                    break;
            //            }
            //    }
            //    Console.WriteLine(line);
            //}


            var totalRisk = 0;
            for (int x = 0; x <= targetX; x++)
            {
                for (int y = 0; y <= targetY; y++)
                {
                    if (x == 0 && y == 0) { }
                    else if (x == targetX && y == targetY) { }
                    else
                        switch (map[x, y])
                        {
                            case RegionType.Rocky:
                                totalRisk += 0;
                                break;
                            case RegionType.Wet:
                                totalRisk += 1;
                                break;
                            case RegionType.Narrow:
                                totalRisk += 2;
                                break;
                            default:
                                break;
                        }
                }
            }
            Console.WriteLine("Total risk is " + totalRisk);

            Console.ReadKey(true);
        }

        static int GeologicIndex(int X, int Y)
        {
            if (X == 0 && Y == 0) return 0;
            if (X == targetX && Y == targetY) return 0;
            if (Y == 0) return X * 16807;
            if (X == 0) return Y * 48271;

            return ErosionLevel(X - 1, Y) * ErosionLevel(X, Y - 1);
        }

        private static int ErosionLevel(int x, int y)
        {
            if (erosionLevels[x, y] == -1)
                erosionLevels[x, y] = (GeologicIndex(x, y) + depth) % 20183;

            return erosionLevels[x, y];
        }

        private static RegionType FindRegionType(int x, int y)
        {
            switch (ErosionLevel(x, y) % 3)
            {
                case 0:
                    return RegionType.Rocky;
                case 1:
                    return RegionType.Wet;
                case 2:
                    return RegionType.Narrow;
                default:
                    throw new Exception("");
            }
        }
    }
}
