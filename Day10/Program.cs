using System;
using System.Collections.Generic;
using System.Linq;

namespace Day10
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = System.IO.File.ReadAllLines("Input.txt")
                        .Select(line => new Star(line));

            var seconds = 1;
            while (true)
            {
                var p = StarsAtTime(input, seconds);
                DisplayStars(p, seconds);
                seconds++;
            }

        }

        static void DisplayStars(IEnumerable<(int X, int Y)> stars, int seconds)
        {
            var minX = stars.Min(star => star.X);
            var minY = stars.Min(star => star.Y);
            var maxX = stars.Max(star => star.X);
            var maxY = stars.Max(star => star.Y);

            if (Math.Abs(maxX - minX) > 100) return;
            if (Math.Abs(maxY - minY) > 100) return;

            var starField = new bool[1 + maxX - minX, 1 + maxY - minY];
            foreach (var (X, Y) in stars)
            {
                starField[X - minX, Y - minY] = true;
            }

            Console.WriteLine("");
            Console.WriteLine(seconds);
            for (int y = 0; y < 1 + maxY - minY; y++)
            {
                var line = "";
                for (int x = 0; x < 1 + maxX - minX; x++)
                {
                    line += starField[x, y] ? 'X' : ' ';
                }
                Console.WriteLine(line);
            }

        }
        static IEnumerable<(int X, int Y)> StarsAtTime(IEnumerable<Star> stars, int seconds)
        {
            return stars.Select(star => PositionAtTime(star, seconds));
        }

        static (int X, int Y) PositionAtTime(Star star, int seconds)
        {
            var x = star.StartX + (star.SpeedX * seconds);
            var y = star.StartY + (star.SpeedY * seconds);
            return (x, y);
        }
    }
}
