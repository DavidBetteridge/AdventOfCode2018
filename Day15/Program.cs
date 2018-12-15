using System.Collections.Generic;
using System.Linq;

namespace Day15
{
    class Program
    {
        static void Main(string[] args)
        {
            var board = Transpose(System.IO.File.ReadLines("Sample.txt").Select(a => a.ToCharArray()).ToArray());
            Display(board);

            var inRange = InRange(board);
            Display(inRange);

            var reachable = Reachable(inRange, 1, 1);
            Display(reachable);

            var distances = Distances(board, 1, 1);
            Display(distances);

            System.Console.ReadKey();
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
