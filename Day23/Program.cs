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
            var nanobots = File.ReadAllLines("Input.txt").Select(line => new Nanobot(line)).ToArray();
            var strongest = nanobots.MaxBy(nb => nb.SignalRadius);
            var inRange = nanobots.Count(n => strongest.SignalRadius >= (n - strongest));

            Console.WriteLine("Part 1 " + inRange);

            var minX = nanobots.Min(n => n.X);
            var minY = nanobots.Min(n => n.Y);
            var minZ = nanobots.Min(n => n.Z);
            var maxX = nanobots.Max(n => n.X);
            var maxY = nanobots.Max(n => n.Y);
            var maxZ = nanobots.Max(n => n.Z);
            var minSignalRadius = nanobots.Min(n => n.SignalRadius);

            var boxSize = (int)Math.Pow(2, 22);
            var (X, Y, Z) = FindHighestBox(nanobots, minX, minY, minZ, maxX, maxY, maxZ, boxSize);

            while (true)
            {
                (X, Y, Z) = FindHighestBox(nanobots, X, Y, Z, X + boxSize, Y + boxSize, Z + boxSize, boxSize / 2);
                boxSize = boxSize / 2;

                if (boxSize == 1)
                {
                    //125532607   you need to Add X to the x to get the correct value.  Some strange off-by-one error going on
                    Console.WriteLine($"Part 2 ({InRangeOf(nanobots, X, Y, Z)}) {DistanceToOrigin(X, Y, Z)}");
                    Console.ReadKey(true);
                }

            }


        }

        private static (int X, int Y, int Z) FindHighestBox(IEnumerable<Nanobot> nanobots, int minX, int minY, int minZ, int maxX, int maxY, int maxZ, int boxSize)
        {
            var highestScore = int.MinValue;
            (int X, int Y, int Z) highestbox = (int.MaxValue, 0, 0);
            for (int x = minX; x <= maxX; x = x + boxSize)
            {
                for (int y = minY; y <= maxY; y = y + boxSize)
                {
                    for (int z = minZ; z <= maxZ; z = z + boxSize)
                    {
                        var boxesScore = FindHighestScoreInBox(nanobots, x, y, z, x + boxSize, y + boxSize, z + boxSize);

                        if (boxesScore == highestScore && DistanceToOrigin(x, y, z) < DistanceToOrigin(highestbox.X, highestbox.Y, highestbox.Z))
                        {
                            highestScore = boxesScore;
                            highestbox = (x, y, z);
                        }

                        if (boxesScore > highestScore)
                        {
                            highestScore = boxesScore;
                            highestbox = (x, y, z);
                        }
                    }
                }
            }

            Console.WriteLine("Highest score = " + highestScore + " (box size is " + boxSize + ")");
            return highestbox;
        }

        private static int FindHighestScoreInBox(IEnumerable<Nanobot> nanobots, int X1, int Y1, int Z1, int X2, int Y2, int Z2)
        {
            var score = 0;

            foreach (var bot in nanobots)
            {
                if (
                    (bot - (X1, Y1, Z1) <= bot.SignalRadius) ||
                    (bot - (X2, Y1, Z1) <= bot.SignalRadius) ||
                    (bot - (X1, Y2, Z1) <= bot.SignalRadius) ||
                    (bot - (X2, Y2, Z1) <= bot.SignalRadius) ||
                    (bot - (X1, Y1, Z2) <= bot.SignalRadius) ||
                    (bot - (X2, Y1, Z2) <= bot.SignalRadius) ||
                    (bot - (X1, Y2, Z2) <= bot.SignalRadius) ||
                    (bot - (X2, Y2, Z2) <= bot.SignalRadius)
                    ) score++;
            }


            return score;
        }

        public static int InRangeOf(IEnumerable<Nanobot> nanobots, int X, int Y, int Z)
        {
            return nanobots.Count(bot => bot - (X, Y, Z) <= bot.SignalRadius);
        }

        public static int DistanceToOrigin(int X, int Y, int Z) => X + Y + Z;
    }
}
