using System;
using System.Collections.Generic;
using System.Linq;

namespace Day15
{
    class Program
    {
        static void Main(string[] args)
        {
            var board = Transpose(System.IO.File.ReadLines("Sample2.txt").Select(a => a.ToCharArray()).ToArray());
            Display(board);

            var startX = 2;
            var startY = 1;

            var inRange = InRange(board);
            Display(inRange);

            var reachable = Reachable(inRange, startX, startY);
            Display(reachable);

            var distances = Distances(board, startX, startY);
            Display(distances);

            var nearest = Nearest(reachable, distances);
            Display(nearest);

            var chosen = Choose(nearest, startX, startY);
            var withChosen = (char[,])board.Clone();
            withChosen[chosen.x, chosen.y] = '+';
            Display(withChosen);

            var shortestPath = Distances(board, chosen.x, chosen.y);
            Display(shortestPath);

            var step = FindStep(shortestPath, startX, startY);
            var withStep = (char[,])board.Clone();
            withStep[startX, startY] = '.';
            withStep[step.X, step.Y] = 'E';
            Display(withStep);

            System.Console.ReadKey();
        }

        private static (int X, int Y) FindStep(int[,] shortestPath, int x, int y)
        {
            var lowests = new List<(int x, int y)>();

            var left = (x > 0) ? shortestPath[x - 1, y] : int.MaxValue;
            var up = (y > 0) ? shortestPath[x, y - 1] : int.MaxValue;
            var right = (x < shortestPath.GetUpperBound(0)) ? shortestPath[x + 1, y] : int.MaxValue;
            var down = (y < shortestPath.GetUpperBound(1)) ? shortestPath[x, y + 1] : int.MaxValue;
            var lowest = Math.Min(Math.Min(Math.Min(left, up), right), down);

            if (up == lowest) return (x, y - 1);
            if (left == lowest) return (x - 1, y);
            if (right == lowest) return (x + 1, y);
            if (down == lowest) return (x, y + 1);

            throw new Exception("no lowest");
        }

        private static (int x, int y) Choose(char[,] board, int startX, int startY)
        {
            for (int y = 0; y < board.GetUpperBound(1); y++)
                for (int x = 0; x < board.GetUpperBound(0); x++)
                    if (board[x, y] == '!') return (x, y);

            throw new System.Exception("Not found");
        }

        private static char[,] Nearest(char[,] board, int[,] distances)
        {
            // Find the @s with the lowest score in distances.
            var lowestScore = int.MaxValue;
            var lowests = new List<(int x, int y)>();
            for (int y = 0; y < board.GetUpperBound(1); y++)
            {
                for (int x = 0; x < board.GetUpperBound(0); x++)
                {
                    if (board[x, y] == '@')
                    {
                        if (distances[x, y] < lowestScore)
                        {
                            lowestScore = distances[x, y];
                            lowests = new List<(int x, int y)> { (x, y) };
                        }
                        else if (distances[x, y] == lowestScore)
                        {
                            lowests.Add((x, y));
                        }
                    }
                }
            }

            var result = (char[,])board.Clone();
            foreach (var (x, y) in lowests)
            {
                result[x, y] = '!';
            }
            return result;
        }

        private static int[,] Distances(char[,] board, int startX, int startY)
        {
            var result = new int[board.GetUpperBound(0), board.GetUpperBound(1)];
            for (int y = 0; y < board.GetUpperBound(1); y++)
                for (int x = 0; x < board.GetUpperBound(0); x++)
                    result[x, y] = int.MaxValue;

            var toExamine = new Stack<(int x, int y)>();

            result[startX, startY] = 0;
            toExamine.Push((startX, startY));

            while (toExamine.Any())
            {
                var (x, y) = toExamine.Pop();

                if (ReadCell(board, x - 1, y) == '.')
                {
                    if (result[x, y] + 1 < result[x - 1, y])
                    {
                        result[x - 1, y] = result[x, y] + 1;
                        toExamine.Push((x - 1, y));
                    }
                }

                if (ReadCell(board, x + 1, y) == '.')
                {
                    if (result[x, y] + 1 < result[x + 1, y])
                    {
                        result[x + 1, y] = result[x, y] + 1;
                        toExamine.Push((x + 1, y));
                    }
                }

                if (ReadCell(board, x, y - 1) == '.')
                {
                    if (result[x, y] + 1 < result[x, y - 1])
                    {
                        result[x, y - 1] = result[x, y] + 1;
                        toExamine.Push((x, y - 1));
                    }
                }

                if (ReadCell(board, x, y + 1) == '.')
                {
                    if (result[x, y] + 1 < result[x, y + 1])
                    {
                        result[x, y + 1] = result[x, y] + 1;
                        toExamine.Push((x, y + 1));
                    }
                }

            }

            return result;
        }

        private static char[,] Reachable(char[,] board, int startX, int startY)
        {
            var result = (char[,])board.Clone();

            var seen = new HashSet<string>();
            var toExamine = new Stack<(int x, int y)>();

            toExamine.Push((startX, startY));

            while (toExamine.Any())
            {
                var (x, y) = toExamine.Pop();
                if (result[x, y] == '?')
                {
                    result[x, y] = '@';
                }
                seen.Add($"{x},{y}");

                if (ReadCell(result, x - 1, y) == '.' && !seen.Contains($"{x - 1},{y}")) toExamine.Push((x - 1, y));
                if (ReadCell(result, x, y - 1) == '.' && !seen.Contains($"{x},{y - 1}")) toExamine.Push((x, y - 1));
                if (ReadCell(result, x + 1, y) == '.' && !seen.Contains($"{x + 1},{y}")) toExamine.Push((x + 1, y));
                if (ReadCell(result, x, y + 1) == '.' && !seen.Contains($"{x},{y + 1}")) toExamine.Push((x, y + 1));

                if (ReadCell(result, x - 1, y) == '?') { result[x - 1, y] = '@'; toExamine.Push((x - 1, y)); }
                if (ReadCell(result, x, y - 1) == '?') { result[x, y - 1] = '@'; toExamine.Push((x, y - 1)); }
                if (ReadCell(result, x + 1, y) == '?') { result[x + 1, y] = '@'; toExamine.Push((x + 1, y)); }
                if (ReadCell(result, x, y + 1) == '?') { result[x, y + 1] = '@'; toExamine.Push((x, y + 1)); }
            }


            return result;
        }

        private static char[,] InRange(char[,] board)
        {
            var result = (char[,])board.Clone();

            for (int y = 0; y <= board.GetUpperBound(1); y++)
            {
                for (int x = 0; x <= board.GetUpperBound(0); x++)
                {
                    if (result[x, y] == '.')
                    {
                        if (ReadCell(result, x + 1, y) == 'G' ||
                            ReadCell(result, x - 1, y) == 'G' ||
                            ReadCell(result, x, y + 1) == 'G' ||
                            ReadCell(result, x, y - 1) == 'G')
                        {
                            result[x, y] = '?';
                        }
                    }
                }
            }

            return result;
        }

        private static char ReadCell(char[,] board, int x, int y)
        {
            if (x >= 0 && y >= 0 && x <= board.GetUpperBound(0) && y <= board.GetUpperBound(1))
            {
                return board[x, y];
            }
            return ' ';
        }

        private static void Display(char[,] board)
        {
            for (int y = 0; y <= board.GetUpperBound(1); y++)
            {
                var line = "";
                for (int x = 0; x <= board.GetUpperBound(0); x++)
                {
                    line += board[x, y];
                }
                System.Console.WriteLine(line);
            }
        }


        private static void Display(int[,] board)
        {
            for (int y = 0; y <= board.GetUpperBound(1); y++)
            {
                var line = "";
                for (int x = 0; x <= board.GetUpperBound(0); x++)
                {
                    if (board[x, y] == int.MaxValue)
                        line += "- ";
                    else
                        line += board[x, y].ToString() + " ";
                }
                System.Console.WriteLine(line);
            }
        }
        private static char[,] Transpose(char[][] matrix)
        {
            int w = matrix.Count();
            int h = matrix[0].Length;

            var result = new char[h, w];

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    result[j, i] = matrix[i][j];
                }
            }

            return result;
        }
    }
}
