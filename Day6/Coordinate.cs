using System;

namespace Day6
{
    class Coordinate
    {
        public int X { get; }
        public int Y { get; }
        public int Index { get; }

        public Coordinate(int x, int y, int index = -1)
        {
            X = x;
            Y = y;
            Index = index;
        }

        public static int operator -(Coordinate a, Coordinate b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);

        internal static Coordinate ParseLine(string line, int index)
        {
            var parts = line.Split(new string[] { ", " }, StringSplitOptions.None);
            return new Coordinate(int.Parse(parts[0]),
                                  int.Parse(parts[1]),
                                  index);
        }
    }
}
