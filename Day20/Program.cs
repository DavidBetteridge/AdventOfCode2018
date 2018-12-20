using System;
using System.IO;

namespace Day20
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = File.ReadAllText("Input.txt");
            var map = new Map();

            var pathWalker = new PathWalker( map);
            pathWalker.WalkPath(path,1);

            var displayer = new DisplayMap();
            displayer.Draw(Console.Out, map);

            Console.ReadKey(true);
        }
    }
}
