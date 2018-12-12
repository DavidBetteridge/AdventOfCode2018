using System;

namespace Day12
{
    //#.#.. => .

    class Rule
    {
        public char L1 { get; set; }
        public char L2 { get; set; }
        public char Node { get; set; }
        public char R1 { get; set; }
        public char R2 { get; set; }
        public char Produces { get; set; }

        public Rule(string line)
        {
            L2 = line[0];
            L1 = line[1];
            Node = line[2];
            R1 = line[3];
            R2 = line[4];
            Produces = line[9];
        }

        internal bool Matches(char v1, char v2, char v3, char v4, char v5)
        {
            return v1 == L2 &&
                   v2 == L1 &&
                   v3 == Node &&
                   v4 == R1 &&
                   v5 == R2;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var input = System.IO.File.ReadAllLines("Input.txt");

            var currentState = ("".PadLeft(20, '.') + input[0].Replace("initial state: ", "") + "".PadLeft(20, '.')).ToCharArray();

            var rules = new Rule[input.Length - 2];
            for (int i = 2; i < input.Length; i++)
            {
                rules[i - 2] = new Rule(input[i]);
            }

            for (int generator = 0; generator < 20; generator++)
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

            var score = 0;
            for (int position = 0; position < currentState.Length; position++)
            {
                if (currentState[position] == '#')
                    score += position - 20;
            }

            Timer.TimeMe(() =>
                        {
                            System.Console.WriteLine($"Part 1 -> {Part1()}");
                        });

            //Timer.TimeMe(() =>
            //{
            //    System.Console.WriteLine($"Part 2 -> {Part2()}");
            //});

            System.Console.ReadKey();
        }

        private static object Part2()
        {
            throw new NotImplementedException();
        }

        private static object Part1()
        {
            throw new NotImplementedException();
        }
    }
}
