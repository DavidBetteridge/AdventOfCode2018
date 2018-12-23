using System;
using System.Collections.Generic;
using System.Linq;

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

        //private static readonly int targetX = 10;
        //private static readonly int targetY = 10;
        //private static readonly int depth = 510;

        public static int[,] erosionLevels { get; private set; }

        static void Main(string[] args)
        {
            var map = new RegionType[targetX + 100, targetY + 100];


            WorkOutRegionTypes(map);

     //       DisplayMap(map);

            WorkOutPathCosts(map);

            TotalRisk(map);

            Console.ReadKey(true);
        }

        class Cost
        {
            public int Climbing { get; set; }
            public int Torch { get; set; }
            public int Neither { get; set; }

            internal int ReadCostType(string costType)
            {
                switch (costType)
                {
                    case "TORCH":
                        return Torch;
                    case "CLIMBING":
                        return Climbing;
                    case "NEITHER":
                        return Neither;
                    default:
                        throw new Exception("Unknown cost type " + costType);
                }
            }
        }

        private static void WorkOutPathCosts(RegionType[,] map)
        {
            var MaxX = targetX + 100;
            var MaxY = targetY + 100;

            var costs = new Cost[MaxX, MaxY];
            costs[targetX, targetY] = new Cost() { Climbing = 7, Neither = 0, Torch = 0 };
            map[targetX, targetY] = RegionType.Rocky;

            var toVisit = new Stack<(int x, int y)>();
            toVisit.Push((targetX - 1, targetY));
            toVisit.Push((targetX + 1, targetY));
            toVisit.Push((targetX, targetY - 1));
            toVisit.Push((targetX, targetY + 1));

            while (toVisit.Any())
            {
                var (x, y) = toVisit.Pop();

                costs[x, y] = new Cost();

                EvaluateCell(map, costs, x, y, MaxX, MaxY);

                //  DisplayCost(map, costs);
                //     Console.ReadKey(true);

                if (x > 0 && ShouldExamine(costs[x, y], costs[x - 1, y], map[x, y]))
                {
                    toVisit.Push((x - 1, y));
                }

                if (x + 1 < MaxX && ShouldExamine(costs[x, y], costs[x + 1, y], map[x, y]))
                {
                    toVisit.Push((x + 1, y));
                }

                if (y + 1 < MaxY && ShouldExamine(costs[x, y], costs[x, y + 1], map[x, y]))
                {
                    toVisit.Push((x, y + 1));
                }

                if (y > 0 && ShouldExamine(costs[x, y], costs[x, y - 1], map[x, y]))
                {
                    toVisit.Push((x, y - 1));
                }
            }

            for (int a = 0; a < 1500; a++)
            {
                for (int x = 0; x <= map.GetUpperBound(0); x++)
                {
                    for (int y = 0; y <= map.GetUpperBound(1); y++)
                    {
                        if (x != targetX || y != targetY)
                            EvaluateCell(map, costs, x, y, MaxX, MaxY);
                    }
                }
            }

        //    DisplayCost(map, costs);
            var part2 = costs[0, 1].Torch + 1;
            Console.WriteLine("Part 2 is " + part2);

        }

        private static void EvaluateCell(RegionType[,] map, Cost[,] costs, int x, int y, int MaxX, int MaxY)
        {

            if (map[x, y] == RegionType.Wet)
            {
                var climbing = FindLowestClimbing(map, costs, x, y, MaxX, MaxY);
                var neither = FindLowestNeither(map, costs, x, y, MaxX, MaxY);

                costs[x, y].Neither = Math.Min(1 + neither, 1 + 7 + climbing);
                costs[x, y].Climbing = Math.Min(1 + climbing, 1 + 7 + neither);
            }

            if (map[x, y] == RegionType.Rocky)
            {
                var climbing = FindLowestClimbing(map, costs, x, y, MaxX, MaxY);
                var torch = FindLowestTorch(map, costs, x, y, MaxX, MaxY);

                costs[x, y].Torch = Math.Min(1 + torch, 1 + 7 + climbing);
                costs[x, y].Climbing = Math.Min(1 + climbing, 1 + 7 + torch);
            }

            if (map[x, y] == RegionType.Narrow)
            {
                var neither = FindLowestNeither(map, costs, x, y, MaxX, MaxY);
                var torch = FindLowestTorch(map, costs, x, y, MaxX, MaxY);

                costs[x, y].Neither = Math.Min(1 + 7 + torch, 1 + neither);
                costs[x, y].Torch = Math.Min(1 + 7 + neither, 1 + torch);
            }


        }


        private static bool ShouldExamine(Cost current, Cost adj, RegionType regionType)
        {
            if (adj == null) return true;

            var result = false;

            return result;
        }

        private static int FindLowestClimbing(RegionType[,] map, Cost[,] costs, int x, int y, int MaxX, int MaxY)
        {
            var result = 10000;
            if (x > 0 && costs[x - 1, y] != null && map[x - 1, y] != RegionType.Narrow) result = Math.Min(result, costs[x - 1, y].Climbing);
            if (x + 1 < MaxX && costs[x + 1, y] != null && map[x + 1, y] != RegionType.Narrow) result = Math.Min(result, costs[x + 1, y].Climbing);
            if (y + 1 < MaxY && costs[x, y + 1] != null && map[x, y + 1] != RegionType.Narrow) result = Math.Min(result, costs[x, y + 1].Climbing);
            if (y > 0 && costs[x, y - 1] != null && map[x, y - 1] != RegionType.Narrow) result = Math.Min(result, costs[x, y - 1].Climbing);
            return result;
        }

        private static int FindLowestTorch(RegionType[,] map, Cost[,] costs, int x, int y, int MaxX, int MaxY)
        {
            var result = 10000;
            if (x > 0 && costs[x - 1, y] != null && map[x - 1, y] != RegionType.Wet) result = Math.Min(result, costs[x - 1, y].Torch);
            if (x + 1 < MaxX && costs[x + 1, y] != null && map[x + 1, y] != RegionType.Wet) result = Math.Min(result, costs[x + 1, y].Torch);
            if (y + 1 < MaxY && costs[x, y + 1] != null && map[x, y + 1] != RegionType.Wet) result = Math.Min(result, costs[x, y + 1].Torch);
            if (y > 0 && costs[x, y - 1] != null && map[x, y - 1] != RegionType.Wet) result = Math.Min(result, costs[x, y - 1].Torch);
            return result;
        }

        private static int FindLowestNeither(RegionType[,] map, Cost[,] costs, int x, int y, int MaxX, int MaxY)
        {
            var result = 10000;
            if (x > 0 && costs[x - 1, y] != null && map[x - 1, y] != RegionType.Rocky) result = Math.Min(result, costs[x - 1, y].Neither);
            if (x + 1 < MaxX && costs[x + 1, y] != null && map[x + 1, y] != RegionType.Rocky) result = Math.Min(result, costs[x + 1, y].Neither);
            if (y + 1 < MaxY && costs[x, y + 1] != null && map[x, y + 1] != RegionType.Rocky) result = Math.Min(result, costs[x, y + 1].Neither);
            if (y > 0 && costs[x, y - 1] != null && map[x, y - 1] != RegionType.Rocky) result = Math.Min(result, costs[x, y - 1].Neither);
            return result;
        }

        private static void WorkOutRegionTypes(RegionType[,] map)
        {
            erosionLevels = new int[map.GetUpperBound(0) + 1, map.GetUpperBound(1) + 1];

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
        }

        private static void DisplayCost(RegionType[,] map, Cost[,] cost)
        {
            Console.WriteLine();
            Console.WriteLine();
            for (int y = 0; y <= map.GetUpperBound(1); y++)
            {
                for (int x = 0; x <= map.GetUpperBound(0); x++)
                {
                    var line = "";
                    if (x == 0 && y == 0)
                        line += 'M';
                    else if (x == targetX && y == targetY)
                        line += 'T';
                    else
                        switch (map[x, y])
                        {
                            case RegionType.Rocky:
                                line += '.';
                                break;
                            case RegionType.Wet:
                                line += '=';
                                break;
                            case RegionType.Narrow:
                                line += '|';
                                break;
                            default:
                                break;
                        }

                    line += "/";
                    Console.Write(line);

                    if (cost[x, y] == null)
                    {
                        Console.Write("??/??/??");
                    }
                    else
                    {
                        Console.Write(cost[x, y].Climbing.ToString("00"));
                        Console.Write("/");
                        Console.Write(cost[x, y].Neither.ToString("00"));
                        Console.Write("/");
                        Console.Write(cost[x, y].Torch.ToString("00"));
                    }
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

        private static void DisplayMap(RegionType[,] map)
        {
            for (int y = 0; y <= map.GetUpperBound(1); y++)
            {
                var line = "";
                for (int x = 0; x <= map.GetUpperBound(0); x++)
                {
                    if (x == 0 && y == 0)
                        line += 'M';
                    else if (x == targetX && y == targetY)
                        line += 'T';
                    else
                        switch (map[x, y])
                        {
                            case RegionType.Rocky:
                                line += '.';
                                break;
                            case RegionType.Wet:
                                line += '=';
                                break;
                            case RegionType.Narrow:
                                line += '|';
                                break;
                            default:
                                break;
                        }
                }
                Console.WriteLine(line);
            }
        }

        private static void TotalRisk(RegionType[,] map)
        {
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
