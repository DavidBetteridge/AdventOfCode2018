using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Day10
{
    class Program
    {
        static void Main(string[] args)
        {
            var sw = new Stopwatch();
            sw.Start();

            var stars = System.IO.File.ReadAllLines("Input.txt")
                        .Select(line => new Star(line))
                        .ToArray();

            var smallestDifferenceX = int.MaxValue;
            var differenceInX = int.MaxValue;
            var seconds = 1;

            do
            {
                smallestDifferenceX = differenceInX;

                var minX = int.MaxValue;
                var maxX = int.MinValue;
                for (int i = 0; i < stars.Count(); i++)
                {
                    var x = stars[i].StartX + (stars[i].SpeedX * seconds);
                    if (x > maxX) maxX = x;
                    if (x < minX) minX = x;
                }
                differenceInX = maxX - minX;
                seconds++;
            }
            while (differenceInX < smallestDifferenceX);

            var finalStars = StarsAtTime(stars, seconds - 2);
            DisplayStars(finalStars, seconds);

            sw.Stop();
            Console.WriteLine($"Time : {sw.ElapsedMilliseconds}ms");

        }

        static void DisplayStars(IEnumerable<(int X, int Y)> stars, int seconds)
        {
            var minX = stars.Min(star => star.X);
            var minY = stars.Min(star => star.Y);
            var maxX = stars.Max(star => star.X);
            var maxY = stars.Max(star => star.Y);

            var starField = new bool[1 + maxX - minX, 1 + maxY - minY];
            foreach (var (X, Y) in stars)
            {
                starField[X - minX, Y - minY] = true;
            }

            var sb = new StringBuilder();

            sb.AppendLine("");
            sb.AppendLine($"Seconds : {seconds}");

            for (int y = 0; y < 1 + maxY - minY; y++)
            {
                var line = "";
                for (int x = 0; x < 1 + maxX - minX; x++)
                {
                    line += starField[x, y] ? 'X' : ' ';
                }
                sb.AppendLine(line);
            }

            Console.Write(sb.ToString());
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
