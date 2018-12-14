using System.Collections.Generic;
using System.Linq;

namespace Day14
{
    class Program
    {
        static void Main(string[] args)
        {
            WorkoutScores(9);
            WorkoutScores(5);
            WorkoutScores(18);
            WorkoutScores(2018);
            WorkoutScores(157901);  //9 4 1 1 1 3 7 1 3 3
        }

        private static void WorkoutScores(int after)
        {
            var currentReceipes = new List<int>();
            currentReceipes.Add(0);
            currentReceipes.Add(1);

            var receipes = new List<int>();
            receipes.Add(3);
            receipes.Add(7);

            while (receipes.Count < 10 + after)
            {
                var nextReceipe = 0;
                foreach (var currentReceipe in currentReceipes)
                {
                    if (currentReceipe >= 0)
                        nextReceipe += receipes[currentReceipe];
                }

                if (nextReceipe > 9)
                {
                    var first = nextReceipe / 10;
                    var second = nextReceipe % 10;
                    receipes.Add(first);
                    receipes.Add(second);
                }
                else
                {
                    receipes.Add(nextReceipe);
                }

                for (int i = 0; i < currentReceipes.Count; i++)
                {
                    var moveBy = receipes[currentReceipes[i]] + 1;
                    currentReceipes[i] = (currentReceipes[i] + moveBy) % receipes.Count;
                }


            }

            System.Console.WriteLine($"{after} {string.Join("", receipes.Skip(after).Take(10))}");
        }

        


    }
}
