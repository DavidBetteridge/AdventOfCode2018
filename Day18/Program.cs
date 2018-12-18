using System;
using System.IO;

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

            for (int minute = 0; minute < 10; minute++)
            {
                cells = NextGeneration(cells);
                DisplayCells(Console.Out, cells);
            }

            Score(cells);
        }

        private static void Score(char[,] cells)
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
            Console.WriteLine($"Trees {wood} x  Lumber {lumber} = {wood*lumber}");
            Console.ReadKey(true);
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
