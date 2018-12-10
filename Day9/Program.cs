using System;
using System.Diagnostics;
using System.Linq;

namespace Day9
{
    class Program
    {
        static void Main(string[] args)
        {
            var sw = new Stopwatch();

          //  var part1 = Solve(476, 71657);
            sw.Start();
            var part2 = Solve(476, 7165700);
            sw.Stop();

          //  Console.WriteLine($"Part 1 ==>  {part1}"); //386018
            Console.WriteLine($"Part 2 ==>  {part2} in {sw.ElapsedMilliseconds}ms"); //3085518618
            Console.ReadKey();
        }

        private static long Solve(int numberOfPlayers, int numberOfMarbles)
        {
            var scores = new long[numberOfPlayers];
            var circle = new Circle(numberOfMarbles);
            var playerNumber = 0;

            for (int marble = 1; marble <= numberOfMarbles; marble++)
            {
                if (marble % 23 != 0)
                {
                    circle.AddMarble(marble);
                }
                else
                {
                    var score = circle.RemoveMarbles(marble);
                    scores[playerNumber] += score;
                }

          //      System.Console.WriteLine($"[{playerNumber + 1}]  {circle}");

                playerNumber = (playerNumber + 1) % numberOfPlayers;
            }

            var highestScore = scores.Max();
            return highestScore;
        }
    }
}
