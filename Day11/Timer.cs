using System;
using System.Diagnostics;

namespace Day11
{
    internal static class Timer
    {
        public static void TimeMe(Action toDo)
        {
            var sw = new Stopwatch();
            sw.Start();
            toDo();
            sw.Stop();

            Console.WriteLine($"Time taken {sw.ElapsedMilliseconds}ms");
        }
    }
}
