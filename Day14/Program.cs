using System.Collections.Generic;
using System.Linq;

namespace Day14
{
    class Program
    {
        static void Main(string[] args)
        {
            Part1(9);
            Part1(5);
            Part1(18);
            Part1(2018);
            Part1(157901);  //9 4 1 1 1 3 7 1 3 3

            Part2("51589");
            Part2("01245");
            Part2("92510");
            Part2("59414");
            Part2("157901");

            System.Console.ReadKey();
        }

        private static void Part2(string lookfor)
        {
            var lookForAsBytes = lookfor.ToCharArray().Select(c => byte.Parse(c.ToString())).ToArray();

            var elf1 = 0;
            var elf2 = 1;

            var recipes = new List<byte>();
            recipes.Add(3);
            recipes.Add(7);

            while (!EndsWith(recipes, lookForAsBytes))
            {
                var nextrecipe = (byte)(recipes[elf1] + recipes[elf2]);

                if (nextrecipe > 9)
                {
                    var first = (byte)(nextrecipe / 10);
                    var second = (byte)(nextrecipe % 10);
                    recipes.Add(first);

                    if (!EndsWith(recipes, lookForAsBytes))
                    {
                        recipes.Add(second);
                    }
                }
                else
                {
                    recipes.Add(nextrecipe);
                }

                elf1 = (elf1 + recipes[elf1] + 1) % recipes.Count;
                elf2 = (elf2 + recipes[elf2] + 1) % recipes.Count;
            }

            System.Console.WriteLine($"{recipes.Count - lookfor.Length}");
        }

        private static bool EndsWith(List<byte> recipesText, byte[] lookForAsBytes)
        {
            var upper = lookForAsBytes.Length;
            if (recipesText.Count < upper) return false;

            for (int i = 0; i < upper; i++)
            {
                if (recipesText[recipesText.Count - upper + i] != lookForAsBytes[i]) return false;
            }
            return true;
        }

        private static void Part1(int after)
        {
            var elf1 = 0;
            var elf2 = 1;

            var recipes = new List<int>();
            recipes.Add(3);
            recipes.Add(7);

            while (recipes.Count < 10 + after)
            {
                var nextrecipe = recipes[elf1] + recipes[elf2];

                if (nextrecipe > 9)
                {
                    var first = nextrecipe / 10;
                    var second = nextrecipe % 10;
                    recipes.Add(first);
                    recipes.Add(second);
                }
                else
                {
                    recipes.Add(nextrecipe);
                }

                elf1 = (elf1 + recipes[elf1] + 1) % recipes.Count;
                elf2 = (elf2 + recipes[elf2] + 1) % recipes.Count;
            }

            System.Console.WriteLine($"After {after} they have {string.Join("", recipes.Skip(after).Take(10))}");
        }
    }
}
