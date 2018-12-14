using System;

namespace Day12
{

    class Program
    {
        static void Main(string[] args)
        {
            Timer.TimeMe(() =>
            {
                System.Console.WriteLine($"Part 1 -> {Part1()}");
            });

            Timer.TimeMe(() =>
            {
                System.Console.WriteLine($"Part 2 -> {Part2()}");
            });

            System.Console.ReadKey();
        }

        private static long Part2()
        {
            // After 185 generators the score increases from 35405 in multiples of 194
            var generation = 50000000000 - 1;
            return 35405 + (194 * (generation - 185));
        }

        private static long Part1()
        {
            var input = System.IO.File.ReadAllLines("Input.txt");

            var currentState = ("".PadLeft(20, '.') + input[0].Replace("initial state: ", "") + "".PadLeft(20, '.')).ToCharArray();

            var rules = new Rule[input.Length - 2];
            for (int i = 2; i < input.Length; i++)
            {
                rules[i - 2] = new Rule(input[i]);
            }

            var numberOfGenerations = 20;
            for (int generation = 0; generation < numberOfGenerations; generation++)
            {
                var previousState = (char[])currentState.Clone();

                for (int position = 2; position < currentState.Length - 4; position++)
                {
                    foreach (var rule in rules)
                    {
                        if (rule.Matches(previousState[position - 2],
                                         previousState[position - 1],
                                         previousState[position],
                                         previousState[position + 1],
                                         previousState[position + 2]
                                         ))
                        {
                            currentState[position] = rule.Produces;
                        }
                    }
                }

            }

            return CalculateScore(currentState);
        }

        private static int CalculateScore(char[] currentState)
        {
            var score = 0;
            for (int position = 0; position < currentState.Length; position++)
            {
                if (currentState[position] == '#')
                    score += position - 20;
            }
            return score;
        }
    }
}
