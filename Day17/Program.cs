using System;
using System.IO;
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
            var scans = System.IO.File.ReadAllLines("input.txt").Select(line => new Scan(line));
            var minY = scans.Min(scan => scan.StartY);
            var maxY = scans.Max(scan => scan.EndY) + 1;
            var minX = scans.Min(scan => scan.StartX) - 1;
            var maxX = scans.Max(scan => scan.EndX) + 2;

            var grid = new char[maxX - minX, maxY];
            InitialiseGrid(grid);
            grid[500 - minX, 0] = Squares.Spring;

            foreach (var scan in scans)
                AddClayToGrid(grid, scan, minX);

            WriteToFile(grid, minX);

            AddWater(grid, minX);
            WriteToFile(grid, minX);
            var part1 = WorkOutScoreForPart1(grid, minY, maxY);
            var part2 = WorkOutScoreForPart2(grid, minY, maxY);

            Console.WriteLine($"Part 1 is {part1}");
            Console.WriteLine($"Part 2 is {part2}");
            Console.ReadKey();
        }

        private static void WriteToFile(char[,] grid, int minX)
        {
            using (var sw = File.CreateText("inputgrid.txt"))
            {
                DisplayGrid(sw, grid, minX);
            }
        }

        private static void AddWater(char[,] grid, int xOffset)
        {
            var x = 500 - xOffset;
            var y = 1;

            FlowDown(grid, x, y);

        }
        private static void FlowDown(char[,] grid, int x, int y)
        {
            if (y >= grid.GetUpperBound(1))
            {
                //Infinite
                grid[x, y] = Squares.WaterFlow;
                return;
            }

            if (grid[x, y + 1] == Squares.WaterFlow)
            {
                //We have hit an existing flow
                grid[x, y] = Squares.WaterFlow;
                return;
            }


            if (grid[x, y + 1] == Squares.Sand)
            {
                FlowDown(grid, x, y + 1);
                if (grid[x, y + 1] == Squares.WaterFlow)
                {
                    grid[x, y] = Squares.WaterFlow;
                    return;
                }
            }

            if (grid[x - 1, y] == Squares.Clay)
            {
                grid[x, y] = Squares.WaterAtRest;
            }
            else
            {
                FlowLeft(grid, x - 1, y);
                if (grid[x, y] != Squares.WaterFlow)
                    grid[x, y] = grid[x - 1, y];
            }

            if (grid[x + 1, y] == Squares.Clay)
            {
                if (grid[x, y] != Squares.WaterFlow)
                    grid[x, y] = Squares.WaterAtRest;
            }
            else
            {
                FlowRight(grid, x + 1, y);
                if (grid[x, y] != Squares.WaterFlow)
                    grid[x, y] = grid[x + 1, y];
            }

            if (grid[x + 1, y] == Squares.WaterFlow)
            {
                var a = x - 1;
                while (grid[a, y] == Squares.WaterAtRest)
                {
                    grid[a, y] = Squares.WaterFlow;
                    a--;
                }
            }

            if (grid[x - 1, y] == Squares.WaterFlow)
            {
                var a = x + 1;
                while (grid[a, y] == Squares.WaterAtRest)
                {
                    grid[a, y] = Squares.WaterFlow;
                    a++;
                }
            }

        }

        private static int WorkOutScoreForPart1(char[,] grid, int minY, int maxY)
        {
            var score = 0;
            for (int y = minY; y < maxY; y++)
            {
                for (int x = 0; x <= grid.GetUpperBound(0); x++)
                {
                    if (grid[x, y] == Squares.WaterAtRest || grid[x, y] == Squares.WaterFlow)
                        score++;
                }
            }
            return score;
        }

        private static int WorkOutScoreForPart2(char[,] grid, int minY, int maxY)
        {
            var score = 0;
            for (int y = minY; y < maxY; y++)
            {
                for (int x = 0; x <= grid.GetUpperBound(0); x++)
                {
                    if (grid[x, y] == Squares.WaterAtRest)
                        score++;
                }
            }
            return score;
        }



        private static void FlowRight(char[,] grid, int x, int y)
        {
            if (grid[x, y + 1] == Squares.WaterFlow) return;

            if (grid[x, y + 1] == Squares.Sand)
            {
                FlowDown(grid, x, y + 1);
                if (grid[x, y + 1] == Squares.WaterFlow)
                {
                    //Infinite
                    grid[x, y] = Squares.WaterFlow;
                    return;
                }
                // else  full
            }

            // Go right
            if (grid[x + 1, y] == Squares.Clay)
            {
                //hit the wall
                grid[x, y] = Squares.WaterAtRest;
                return;
            }


            FlowRight(grid, x + 1, y);

            if (grid[x, y] != Squares.WaterFlow)
                grid[x, y] = grid[x + 1, y];

        }

        private static void FlowLeft(char[,] grid, int x, int y)
        {
            if (grid[x, y + 1] == Squares.WaterFlow) return;

            if (grid[x, y + 1] == Squares.Sand)
            {
                FlowDown(grid, x, y + 1);
                if (grid[x, y + 1] == Squares.WaterFlow)
                {
                    //Infinite
                    grid[x, y] = Squares.WaterFlow;
                    return;
                }
                // else  full
            }

            // Go left
            if (grid[x - 1, y] == Squares.Clay)
            {
                //hit the wall
                grid[x, y] = Squares.WaterAtRest;
                return;
            }

            FlowLeft(grid, x - 1, y);
            grid[x, y] = grid[x - 1, y];
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
        private static void DisplayGrid(TextWriter textWriter, char[,] grid, int xOffset)
        {
            textWriter.WriteLine("");

            for (int i = 3; i >= 0; i--)
            {
                var heading = "      ";
                for (int x = xOffset; x <= xOffset + grid.GetUpperBound(0); x++)
                {
                    heading += (x / (int)(Math.Pow(10, i))) % 10;
                }
                textWriter.WriteLine(heading);
            }

            for (int y = 0; y <= grid.GetUpperBound(1); y++)
            {
                var line = y.ToString("0000") + "  ";
                for (int x = 0; x <= grid.GetUpperBound(0); x++)
                {
                    line += grid[x, y];
                }
                textWriter.WriteLine(line);
            }
        }
    }
}
