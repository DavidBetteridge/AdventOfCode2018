using System;
using System.Collections.Generic;
using System.Text;

namespace Day23
{
    class Nanobot
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int Z { get; set; }
        public int SignalRadius { get; set; }

        public Nanobot(string line)
        {
            var parser = new Parser(line);
            parser.Match("pos=<");
            X = parser.ReadNextInt();
            parser.Match(",");
            Y = parser.ReadNextInt();
            parser.Match(",");
            Z = parser.ReadNextInt();
            parser.Match(">, r=");
            SignalRadius = parser.ReadNextInt();
        }

        public static int operator -(Nanobot a, Nanobot b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z);
    }
}
