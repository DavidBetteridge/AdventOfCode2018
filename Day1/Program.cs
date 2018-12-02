using System;
using System.Collections.Generic;
using System.Linq;

namespace Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = System.IO.File.ReadAllLines("Frequencies.txt").Select(a => int.Parse(a));
            var total = lines.Sum();
            Console.WriteLine(total);

            var totalsSeen = new HashSet<int>();
            var runningTotal = 0;

            foreach (var change in Cycle(lines))
            {
                runningTotal += change;
                if (totalsSeen.Contains(runningTotal))
                {
                    Console.WriteLine(runningTotal);
                }
                else
                {
                    totalsSeen.Add(runningTotal);
                }
            }

            //81972


        }


        public static IEnumerable<int> Cycle(IEnumerable<int> numbers)
        {
            while (true)
            {
                foreach (var item in numbers)
                {
                    yield return item;
                }
            }
        }
    }
}
