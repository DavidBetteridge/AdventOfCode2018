using System;
using System.Linq;

namespace Day9
{
    class Program
    {
        static void Main(string[] args)
        {
            var part1 = Solve(476, 71657);
            var part2 = Solve(476, 71657 * 100);

            Console.WriteLine($"Part 1 ==>  {part1}"); //386018
            Console.WriteLine($"Part 2 ==>  {part2}"); //3085518618
            Console.ReadKey();
        }

        private static long Solve(int numberOfPlayers, int lastMarble)
        {
            var scores = new long[numberOfPlayers];
            var circle = new Circle(lastMarble);
            var playerNumber = 0;
            for (int marble = 1; marble <= lastMarble; marble++)
            {
                if (marble % 23 != 0)
                {
                    circle.AddMarble(marble);
                }
                else
                {
                    var score = circle.RemoveMarbles(marble);
                    scores[playerNumber] += score;
                    //  Console.WriteLine(score);
                }

                //   System.Console.WriteLine($"[{playerNumber}]  {circle}");

                playerNumber = (playerNumber + 1) % numberOfPlayers;
            }

            var highestScore = scores.Max();
            return highestScore;
        }
    }
}
