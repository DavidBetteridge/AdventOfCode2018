using System;

namespace Day25
{
    internal class Point
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Z { get; private set; }
        public int T { get; private set; }

        public Constellation Constellation { get; set; }

        public Point(string line)
        {
            var parts = line.Replace(" ", "").Split(',');
            if (parts.Length != 4) throw new System.Exception("The line is in the wrong format! " + line);

            X = int.Parse(parts[0]);
            Y = int.Parse(parts[1]);
            Z = int.Parse(parts[2]);
            T = int.Parse(parts[3]);
        }

        public static int operator -(Point a, Point b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z) + Math.Abs(a.T - b.T);
    }
}