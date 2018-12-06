using System.Collections.Generic;
using System.Linq;

namespace Day6
{
    class Program
    {
        static void Main(string[] args)
        {
            var coordinates = System.IO.File.ReadAllLines("Coordinates.txt")
                                            .Select((line, index) => Coordinate.ParseLine(line, index))
                                            .ToArray();

            var minX = coordinates.Min(c => c.X);
            var minY = coordinates.Min(c => c.Y);
            var maxX = coordinates.Max(c => c.X);
            var maxY = coordinates.Max(c => c.Y);

            var nearest = new List<int>();
            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    nearest.Add(Closest(x, y, coordinates));
                }
            }

            //var nearest = Enumerable.Range(minX, maxX - minX)
            //                .SelectMany(x => Enumerable.Range(minY, maxY - minY)
            //                .Select(y => Closest(x, y, coordinates)));
            var bbbb = nearest.Count();

            var infiniteTop = Enumerable.Range(minX, maxX - minX).Select(x => Closest(x, minY, coordinates)).Distinct();
            var infiniteBottom = Enumerable.Range(minX, maxX - minX).Select(x => Closest(x, maxY, coordinates)).Distinct();
            var infiniteLeft = Enumerable.Range(minY, maxY - minY).Select(y => Closest(minX, y, coordinates)).Distinct();
            var infiniteRight = Enumerable.Range(minY, maxY - minY).Select(y => Closest(maxX, y, coordinates)).Distinct();
            var infinites = infiniteTop.Union(infiniteBottom).Union(infiniteLeft).Union(infiniteRight);


            var grouped = nearest.GroupBy(a => a);
            bbbb = grouped.Count();

            var possibles = new List<IGrouping<int,int>>();
            foreach (var item in grouped)
            {
                if (!infinites.Any(i => item.Key == i))
                {
                    possibles.Add(item);
                }
            }

                       
            var sorted = possibles.OrderByDescending(a => a.Count());
            //bbbb = sorted.Count();

            var part1 = sorted.First().Count();

        }

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
