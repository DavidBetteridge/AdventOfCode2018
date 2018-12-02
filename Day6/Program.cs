using System.Collections.Generic;
using System.Linq;

namespace Day6
{
    class Program
    {
        static void Main(string[] args)
        {
            Part2();
        }

        private static void Part2()
        {
            var coordinates = System.IO.File.ReadAllLines("Coordinates.txt")
                                            .Select((line, index) => Coordinate.ParseLine(line, index))
                                            .ToArray();

            const int TargetDistance = 10000;
            var numberOfCoordinates = coordinates.Count();
            var maxGridSize = TargetDistance / numberOfCoordinates;

            var minX = coordinates.Min(c => c.X) - maxGridSize;
            var minY = coordinates.Min(c => c.Y) - maxGridSize;
            var maxX = coordinates.Max(c => c.X) + maxGridSize;
            var maxY = coordinates.Max(c => c.Y) + maxGridSize;

            int TotalDistanceTo(Coordinate coordinate) => coordinates.Sum(otherCoordinate => coordinate - otherCoordinate);

            var regionSize = Enumerable.Range(minX, maxX - minX)
                                        .SelectMany(x => Enumerable.Range(minY, maxY - minY)
                                        .Select(y => TotalDistanceTo(new Coordinate(x, y))))
                                        .Count(distance => distance < TargetDistance);

        }
        private static void Part1()
        {
            var coordinates = System.IO.File.ReadAllLines("Coordinates.txt")
                                            .Select((line, index) => Coordinate.ParseLine(line, index))
                                            .ToArray();

            var minX = coordinates.Min(c => c.X);
            var minY = coordinates.Min(c => c.Y);
            var maxX = coordinates.Max(c => c.X);
            var maxY = coordinates.Max(c => c.Y);

            var nearest = Enumerable.Range(minX, maxX - minX)
                            .SelectMany(x => Enumerable.Range(minY, maxY - minY)
                            .Select(y => Closest(x, y, coordinates)));

            var infiniteTop = Enumerable.Range(minX, maxX - minX).Select(x => Closest(x, minY, coordinates)).Distinct();
            var infiniteBottom = Enumerable.Range(minX, maxX - minX).Select(x => Closest(x, maxY, coordinates)).Distinct();
            var infiniteLeft = Enumerable.Range(minY, maxY - minY).Select(y => Closest(minX, y, coordinates)).Distinct();
            var infiniteRight = Enumerable.Range(minY, maxY - minY).Select(y => Closest(maxX, y, coordinates)).Distinct();
            var infinite = infiniteTop.Union(infiniteBottom).Union(infiniteLeft).Union(infiniteRight);

            var part1 = nearest
                            .GroupBy(Identity)
                            .Where(item => !infinite.Any(i => item.Key == i))
                            .OrderByDescending(a => a.Count())
                            .First()
                            .Count();
        }

        static T Identity<T>(T thing) => thing;




        private static int Closest(int x, int y, IEnumerable<Coordinate> coordinates)
        {
            var examining = new Coordinate(x, y, -1);

            var closestDistance = int.MaxValue;
            var closestIndex = -1;
            foreach (var coordinate in coordinates)
            {
                var distance = coordinate - examining;
                if (distance == closestDistance)
                {
                    closestIndex = -1;  //A tie
                }

                if (distance < closestDistance)
                {
                    closestIndex = coordinate.Index;
                    closestDistance = distance;
                }
            }

            return closestIndex;
        }
    }
}
