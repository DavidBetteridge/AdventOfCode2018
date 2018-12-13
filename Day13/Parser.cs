using System.Collections.Generic;
using System.Linq;

namespace Day13
{
    class Parser
    {
        public char[,] Convert(char[][] matrix)
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

        public void Load(out char[,] cells, out List<Cart> carts)
        {
            var file = System.IO.File.ReadAllLines("Input.txt").Select(line => line.ToCharArray()).ToArray();
            cells = Convert(file);
            carts = new List<Cart>();
            for (int y = 0; y <= cells.GetUpperBound(1); y++)
            {
                for (int x = 0; x <= cells.GetUpperBound(0); x++)
                {

                    switch (cells[x, y])
                    {
                        case '<':
                            carts.Add(new Cart() { X = x, Y = y, CurrentDirection = Direction.West, NextTurn = Turn.Left });
                            cells[x, y] = '-';
                            break;

                        case '>':
                            carts.Add(new Cart() { X = x, Y = y, CurrentDirection = Direction.East, NextTurn = Turn.Left });
                            cells[x, y] = '-';
                            break;

                        case '^':
                            carts.Add(new Cart() { X = x, Y = y, CurrentDirection = Direction.North, NextTurn = Turn.Left });
                            cells[x, y] = '|';
                            break;

                        case 'v':
                            carts.Add(new Cart() { X = x, Y = y, CurrentDirection = Direction.South, NextTurn = Turn.Left });
                            cells[x, y] = '|';
                            break;

                        default:
                            break;
                    }
                }
            }
        }


    }
}
