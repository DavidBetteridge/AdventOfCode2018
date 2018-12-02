using System;
using System.Linq;

namespace Day2
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = System.IO.File.ReadAllLines("Boxes.txt");

            var groupedLines = lines.Select(line => line.GroupBy(c => c));
            var numberOfTwo = groupedLines.Where(line => line.Any(c => c.Count() == 2)).Count();
            var numberOfThree = groupedLines.Where(line => line.Any(c => c.Count() == 3)).Count();
            var answer = numberOfTwo * numberOfThree;
            Console.WriteLine(answer);

            var pairs = from l1 in lines
                        from l2 in lines
                        where Distance(l1, l2) == 1
                        select RemoveDifferences(l1, l2);

            Console.WriteLine(pairs.First());
        }

        private static int Distance(string line1, string line2) =>
            line1
                .Zip(line2, (a, b) => (a, b))
                .Count(ab => ab.a != ab.b);
        private static string RemoveDifferences(string line1, string line2) =>
            string.Join("", line1
                            .Zip(line2, (a, b) => (a, b))
                            .Where(ab => ab.a == ab.b)
                            .Select(ab => ab.a));
    }
}
