using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day18
{
    class Acre
    {
        public const char Tree = '|';
        public const char Open = '.';
        public const char Lumberyard = '#';
    }
    class Program
    {
        static void Main(string[] args)
        {
            //1000000000
            var lines = File.ReadAllLines("Input.txt");
            var rows = lines.Length;
            var columns = lines[0].Length;
            var cells = new char[columns, rows];



            for (int row = 0; row < lines.Length; row++)
            {
                var line = lines[row];
                for (int column = 0; column < line.Length; column++)
                {
                    cells[column, row] = line[column];
                }
            }

            var seenBoards = new Dictionary<string, int>();
            var cycle = new Dictionary<string, int>();
            for (int minute = 1; minute < 20000; minute++)
            {
                cells = NextGeneration(cells);

                if (cycle.Count < 28)
                {
                    var hash = Hash(cells);
                    if (seenBoards.TryGetValue(hash, out var lastSeen))
                    {
                        if (!cycle.ContainsKey(hash))
                            cycle.Add(hash, Score(cells));
                    }
                    else
                    {
                        seenBoards.Add(hash, minute);
                    }
                }
            }

            Predict(1000000000, cycle);
            Console.ReadKey(true);
        }

        static void Predict(long target, Dictionary<string, int> cycle)
        {
            const int cycleSize = 28;
            var a = target - 1000;
            var b = a % cycleSize;

            var score = cycle.Values.ToArray()[b + 12];
            Console.WriteLine($"Predict {target} is {score}");
        }
        private static string Hash(char[,] cells)
        {
            var rows = cells.GetUpperBound(1);
            var columns = cells.GetUpperBound(0);
            var result = "";
            for (int row = 0; row <= rows; row++)
                for (int column = 0; column <= columns; column++)
                    result += cells[column, row];

            return result;
        }

        private static int Score(char[,] cells)
        {
            var rows = cells.GetUpperBound(1);
            var columns = cells.GetUpperBound(0);
            var wood = 0;
            var lumber = 0;
            for (int row = 0; row <= rows; row++)
            {
                for (int column = 0; column <= columns; column++)
                {
                    if (cells[column, row] == Acre.Lumberyard) lumber++;
                    if (cells[column, row] == Acre.Tree) wood++;
                }
            }
            //  Console.WriteLine($"Trees {wood} x  Lumber {lumber} = {wood * lumber}");
            return wood * lumber;

            //Console.ReadKey(true);
        }

        private static char[,] NextGeneration(char[,] cells)
        {
            var rows = cells.GetUpperBound(1);
            var columns = cells.GetUpperBound(0);
            var result = new char[columns + 1, rows + 1];

            for (int row = 0; row <= rows; row++)
            {
                for (int column = 0; column <= columns; column++)
                {
                    var numberOfTrees = CountAdjacentSquares(cells, column, row, Acre.Tree);
                    var numberOfOpen = CountAdjacentSquares(cells, column, row, Acre.Open);
                    var numberOfLumberyards = CountAdjacentSquares(cells, column, row, Acre.Lumberyard);

                    switch (cells[column, row])
                    {
                        case Acre.Open:
                            if (numberOfTrees >= 3)
                                result[column, row] = Acre.Tree;
                            else
                                result[column, row] = cells[column, row];
                            break;

                        case Acre.Tree:
                            if (numberOfLumberyards >= 3)
                                result[column, row] = Acre.Lumberyard;
                            else
                                result[column, row] = cells[column, row];
                            break;

                        case Acre.Lumberyard:
                            if (numberOfLumberyards >= 1 && numberOfTrees >= 1)
                                result[column, row] = Acre.Lumberyard;
                            else
                                result[column, row] = Acre.Open;
                            break;

                        default:
                            break;
                    }

                }
            }

            return result;
        }

        private static int CountAdjacentSquares(char[,] cells, int column, int row, char what)
        {
            var result = 0;

            if (column > 0)
            {
                if (row > 0 && cells[column - 1, row - 1] == what) result++;
                if (cells[column - 1, row] == what) result++;
                if (row < cells.GetUpperBound(1) && cells[column - 1, row + 1] == what) result++;
            }

            if (row > 0 && cells[column, row - 1] == what) result++;
            if (row < cells.GetUpperBound(1) && cells[column, row + 1] == what) result++;

            if (column < cells.GetUpperBound(0))
            {
                if (row > 0 && cells[column + 1, row - 1] == what) result++;
                if (cells[column + 1, row] == what) result++;
                if (row < cells.GetUpperBound(1) && cells[column + 1, row + 1] == what) result++;
            }

            return result;
        }

        private static void DisplayCells(TextWriter textWriter, char[,] cells)
        {
            for (int row = 0; row <= cells.GetUpperBound(1); row++)
            {
                var line = "";
                for (int column = 0; column <= cells.GetUpperBound(0); column++)
                {
                    line += cells[column, row];
                }
                textWriter.WriteLine(line);
            }
            Console.ReadKey(true);
        }
    }
}
