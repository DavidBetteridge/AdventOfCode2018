using System;
using System.Collections.Generic;
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
            Console.WriteLine($"Part 2 ==>  {part2}"); 
            Console.ReadKey();
        }

        private static int Solve(int numberOfPlayers, int lastMarble)
        {
            var scores = new int[numberOfPlayers];
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
                    //    Console.WriteLine(score);
                }

                //   System.Console.WriteLine($"[{playerNumber}]  {circle}");

                playerNumber = (playerNumber + 1) % numberOfPlayers;
            }

            var highestScore = scores.Max();
            return highestScore;
        }
    }

    class Circle
    {
        private readonly List<int> _marbles;
        private int _currentPosition;
        public Circle(int maxSize)
        {
            _marbles = new List<int>() { 0 };
            _currentPosition = 1;
        }

        public void AddMarble(int marble)
        {
            if (_marbles.Count == 1)
            {
                _marbles.Insert(1, marble);
                _currentPosition = 1;
            }
            else
            {
                var locationToInsert = (_currentPosition + 1) % _marbles.Count;
                _currentPosition = locationToInsert + 1;
                _marbles.Insert(_currentPosition, marble);
            }
        }

        public override string ToString()
        {
            var display = "";
            for (int i = 0; i < _marbles.Count; i++)
            {
                if (i == _currentPosition)
                    display += $"({_marbles[i]}) ";
                else
                    display += _marbles[i] + " ";
            }
            return display;
        }

        internal int RemoveMarbles(int marble)
        {
            var score = marble;

            //  var toRemove = (_currentPosition - 7) % _marbles.Count;
            var toRemove = nfmod(_currentPosition - 7, _marbles.Count);
            score += _marbles[toRemove];
            _marbles.Remove(_marbles[toRemove]);

            _currentPosition = toRemove % _marbles.Count;

            return score;
        }

        int nfmod(int a, int b)
        {
            return a - b * (int)Math.Floor(a / (double)b);
        }
    }
}
