using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day23
{
    class Program
    {
        static void Main(string[] args)
        {
            var nanobots = File.ReadAllLines("Input.txt").Select(line => new Nanobot(line));
            var strongest = nanobots.MaxBy(nb => nb.SignalRadius);
            var inRange = nanobots.Count(n => strongest.SignalRadius >= (n - strongest));

            Console.WriteLine("Part 1 " + inRange);
        }


    }

    static class Extensions
    {

        public static T MaxBy<T>(this IEnumerable<T> source, Func<T, int> action)
        {
            var bestMatch = source.FirstOrDefault();
            var best = action(bestMatch);

            foreach (var item in source)
            {
                var score = action(item);
                if (score > best)
                {
                    best = score;
                    bestMatch = item;
                }
            }
            return bestMatch;
        }
    }
}
