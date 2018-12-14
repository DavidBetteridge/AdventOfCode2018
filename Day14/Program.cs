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

            Lookfor("51589");
            Lookfor("01245");
            Lookfor("92510");
            Lookfor("59414");
            Lookfor("157901");

            System.Console.ReadKey();
        }

        private static void Lookfor(string lookfor)
        {
            var compareWith = lookfor.ToCharArray().Select(a => byte.Parse(a.ToString())).ToArray();

            var elf1 = 0;
            var elf2 = 1;

            var receipes = new List<byte>(int.MaxValue / 10)
            {
                3,
                7
            };

            var receipesText = "37".PadLeft(lookfor.Length,'0');

            while (true)
            {
                var nextReceipe = receipes[elf1] + receipes[elf2];

                if (nextReceipe > 9)
                {
                    var first = nextReceipe / 10;
                    var second = nextReceipe % 10;

                    receipes.Add((byte)first);
                    receipesText = receipesText.Substring(1) + first.ToString();

                    if (receipesText == lookfor)
                    {
                        System.Console.WriteLine($"{receipes.Count-lookfor.Length}");
                        return;
                    }

                    receipes.Add((byte)second);
                    receipesText = receipesText.Substring(1) + second.ToString();

                    if (receipesText == lookfor)
                    {
                        System.Console.WriteLine($"{receipes.Count - lookfor.Length}");
                        return;
                    }
                }
                else
                {
                    receipes.Add((byte)nextReceipe);

                    receipesText = receipesText.Substring(1) + nextReceipe.ToString();

                    if (receipesText == lookfor)
                    {
                        System.Console.WriteLine($"{receipes.Count - lookfor.Length}");
                        return; 
                    }
                }

                elf1 = (elf1 + receipes[elf1] + 1) % receipes.Count;
                elf2 = (elf2 + receipes[elf2] + 1) % receipes.Count;
            }
        }

        private static bool EndWith(List<byte> receipes, byte[] compareWith)
        {
            if (receipes.Count() < compareWith.Length) return false;
            for (int i = 1; i <= compareWith.Length; i++)
                if (compareWith[compareWith.Length - i] != receipes[receipes.Count() - i]) return false;

            return true;
        }

        private static void WorkoutScores(int after)
        {

            var elf1 = 0;
            var elf2 = 1;

            var receipes = new List<int>();
            receipes.Add(3);
            receipes.Add(7);

            while (receipes.Count < 10 + after)
            {
                var nextReceipe = receipes[elf1] + receipes[elf2];

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

                elf1 = (elf1 + receipes[elf1] + 1) % receipes.Count;
                elf2 = (elf2 + receipes[elf2] + 1) % receipes.Count;


            }

            System.Console.WriteLine($"After {after} they have {string.Join("", receipes.Skip(after).Take(10))}");
        }




    }
}
