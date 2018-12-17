using System;
using System.Linq;

namespace Day17
{
    class Program
    {
        class Squares
        {
            public const char Spring = '+';
            public const char Sand = '.';
            public const char Clay = '#';
            public const char WaterAtRest = '~';
            public const char WaterFlow = '|';
        }
        static void Main(string[] args)
        {
            var scans = System.IO.File.ReadAllLines("Sample.txt").Select(line => new Scan(line));
            var minY = scans.Min(scan => scan.StartY);
            var maxY = scans.Max(scan => scan.EndY) + 1;
            var minX = scans.Min(scan => scan.StartX) - 1;
            var maxX = scans.Max(scan => scan.EndX) + 1;

            var grid = new char[maxX - minX, maxY];
            InitialiseGrid(grid);
            grid[500 - minX, 0] = Squares.Spring;

            foreach (var scan in scans)
                AddClayToGrid(grid, scan, minX);

            DisplayGrid(grid);

            var tileCount = 0;
            while (true)
            {
                AddWater(grid, minX);
                DisplayGrid(grid);
                tileCount++;
            }

        }

        private static void AddWater(char[,] grid, int xOffset)
        {
            var x = 500 - xOffset;
            var y = 1;

            FlowDown(grid, x, y);

        }
        private static bool? FlowDown(char[,] grid, int x, int y)
        {
            if (y >= grid.GetUpperBound(1))
            {
                grid[x, y] = Squares.WaterFlow;
                return null;//null means infinite
            }

            if (grid[x, y + 1] == Squares.Clay) return false;
            if (grid[x, y + 1] == Squares.Sand) grid[x, y] = Squares.WaterFlow;
            y++;

            var downResult = FlowDown(grid, x, y);
            if (!downResult.HasValue) return null;
            if (downResult.Value)
            {
                return true;
            }

            if (grid[x, y] == Squares.Sand || grid[x, y] == Squares.WaterFlow)
            {
                grid[x, y] = Squares.WaterAtRest;
                return true;
            }

            if (FlowLeft(grid, x, y)) return true;
            if (FlowRight(grid, x, y)) return true;

            return false;
        }

        private static int WorkOutScoreForPart1(char[,] grid)
        {
            var score = 0;
            for (int y = 0; y <= grid.GetUpperBound(1); y++)
            {
                for (int x = 0; x <= grid.GetUpperBound(0); x++)
                {
                    if (grid[x, y] == Squares.WaterAtRest || grid[x, y] == Squares.WaterFlow)
                        score++;
                }
            }
            return score;
        }

        private static bool FlowRight(char[,] grid, int x, int y)
        {
            if (grid[x + 1, y] == Squares.Clay) return false;
            x++;

            var downResult = FlowDown(grid, x, y);
            if (!downResult.HasValue) return false;
            if (downResult.Value) return true;

            if (grid[x, y] == Squares.Sand || grid[x, y] == Squares.WaterFlow)
            {
                grid[x, y] = Squares.WaterAtRest;
                return true;
            }

            if (grid[x, y] == Squares.WaterAtRest)
            {
                return FlowRight(grid, x, y);
            }

            return false;

        }

        private static bool FlowLeft(char[,] grid, int x, int y)
        {
            if (grid[x - 1, y] == Squares.Clay) return false;
            x--;

            var downResult = FlowDown(grid, x, y);
            if (!downResult.HasValue) return false;
            if (downResult.Value) return true;

            if (grid[x, y] == Squares.Sand || grid[x, y] == Squares.WaterFlow)
            {
                grid[x, y] = Squares.WaterAtRest;
                return true;
            }

            if (grid[x, y] == Squares.WaterAtRest)
            {
                return FlowLeft(grid, x, y);
            }

            return false;
        }



        private static void AddClayToGrid(char[,] grid, Scan scan, int xOffset)
        {
            for (int x = scan.StartX; x <= scan.EndX; x++)
                for (int y = scan.StartY; y <= scan.EndY; y++)
                    grid[x - xOffset, y] = Squares.Clay;
        }

        private static void InitialiseGrid(char[,] grid)
        {
            for (int y = 0; y <= grid.GetUpperBound(1); y++)
                for (int x = 0; x <= grid.GetUpperBound(0); x++)
                    grid[x, y] = Squares.Sand;

        }
        private static void DisplayGrid(char[,] grid)
        {
            Console.WriteLine("");

            for (int y = 0; y <= grid.GetUpperBound(1); y++)
            {
                var line = "";
                for (int x = 0; x <= grid.GetUpperBound(0); x++)
                {
                    line += grid[x, y];
                }
                Console.WriteLine(line);
            }
            Console.ReadKey(true);
        }
    }
}
