using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day25
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Part1("Sample1.txt") != 2) Console.WriteLine("Test 1 Failed");
            if (Part1("Sample2.txt") != 4) Console.WriteLine("Test 2 Failed");
            if (Part1("Sample3.txt") != 3) Console.WriteLine("Test 3 Failed");
            if (Part1("Sample4.txt") != 8) Console.WriteLine("Test 4 Failed");
            Console.WriteLine("Part 1 is " + Part1("Input.txt"));
        }

        private static int Part1(string filename)
        {
            var coords = File.ReadAllLines(filename).Select(line => new Point(line)).ToArray();
            var constellations = new List<Constellation>();

            for (int i = 0; i < coords.Count(); i++)
            {
                var left = coords[i];
                for (int j = i + 1; j < coords.Count(); j++)
                {
                    var right = coords[j];
                    if (right - left <= 3)
                    {
                        if (left.Constellation == null && right.Constellation == null)
                        {
                            left.Constellation = new Constellation();
                            constellations.Add(left.Constellation);
                            left.Constellation.Points.Add(left);
                            left.Constellation.Points.Add(right);
                            right.Constellation = left.Constellation;
                        }
                        else if (left.Constellation == null && right.Constellation != null)
                        {
                            left.Constellation = right.Constellation;
                            left.Constellation.Points.Add(left);
                        }
                        else if (left.Constellation != null && right.Constellation == null)
                        {
                            right.Constellation = left.Constellation;
                            left.Constellation.Points.Add(right);
                        }
                        else if (left.Constellation != null && right.Constellation == left.Constellation)
                        {
                            //Already linked
                        }
                        else if (left.Constellation != null && right.Constellation != null)
                        {
                            constellations.Remove(right.Constellation);
                            left.Constellation.Points.AddRange(right.Constellation.Points);

                            foreach (var p in left.Constellation.Points)
                                p.Constellation = left.Constellation;
                        }
                    }

                }

                if (left.Constellation == null)
                {
                    // On it's own
                    left.Constellation = new Constellation();
                    left.Constellation.Points.Add(left);
                    constellations.Add(left.Constellation);
                }

            }



            Console.WriteLine($"{filename} is {constellations.Count()}");
            return constellations.Count();
        }
    }
}
