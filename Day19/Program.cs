using System;
using System.Collections.Generic;
using System.IO;

namespace Day19
{
    class Program
    {
        static readonly int[] registers = new int[6];

        static readonly Dictionary<string, Action<int, int, int>> Actions = new Dictionary<string, Action<int, int, int>>()
        {
            ["addr"] = addr,
            ["addi"] = addi,
            ["mulr"] = mulr,
            ["muli"] = muli,
            ["banr"] = banr,
            ["bani"] = bani,
            ["borr"] = borr,
            ["bori"] = bori,
            ["setr"] = setr,
            ["seti"] = seti,
            ["gtir"] = gtir,
            ["gtri"] = gtri,
            ["gtrr"] = gtrr,
            ["eqir"] = eqir,
            ["eqri"] = eqri,
            ["eqrr"] = eqrr,
        };

        static void Main(string[] args)
        {
            var ipRegister = 0;
            var ipPointer = 0;
            var allLines = ReadFile();

            registers[0] = 1;

            while (ipPointer < allLines.Length)
            {
                var line = allLines[ipPointer];

                var parser = new Parser(line);
                if (parser.TryMatch("#ip "))
                {
                    ipRegister = parser.ReadNextInt();
                    parser.Match(' ');
                    ipPointer = registers[ipRegister];
                }

                var t = $"ip={ipPointer} [";
                registers[ipRegister] = ipPointer;
                for (int i = 0; i < 6; i++)
                    t += ($"{registers[i]} ");
                t += "]";

                var command = parser.ReadNextWord();
                parser.Match(' ');
                var A = parser.ReadNextInt();
                parser.Match(' ');
                var B = parser.ReadNextInt();
                parser.Match(' ');
                var C = parser.ReadNextInt();

                var action = Actions[command];
                action(A, B, C);

                ipPointer = registers[ipRegister] + 1;

                t += $" {line} [";
                for (int i = 0; i < 6; i++)
                    t += ($"{registers[i]} ");
                t += "]";
                //Console.WriteLine(t);
            }

            var l = "";
            for (int i = 0; i < 6; i++)
                l += ($"{registers[i]} ");
            Console.WriteLine(l);

            Console.ReadKey(true);
        }

        private static string[] ReadFile()
        {
            var allLines = File.ReadAllLines("Input.txt");

            var nextLine = "";
            var processedLines = new List<string>();
            foreach (var line in allLines)
            {
                if (line.StartsWith("#"))
                {
                    nextLine = line + " ";
                }
                else
                {
                    processedLines.Add(nextLine + line);
                    nextLine = "";
                }
            }

            return processedLines.ToArray();
        }

        static void addr(int A, int B, int C)
        {
            registers[C] = registers[A] + registers[B];
        }

        static void addi(int A, int B, int C)
        {
            registers[C] = registers[A] + B;
        }

        static void mulr(int A, int B, int C)
        {
            registers[C] = registers[A] * registers[B];
        }

        static void muli(int A, int B, int C)
        {
            registers[C] = registers[A] * B;
        }
        static void banr(int A, int B, int C)
        {
            registers[C] = registers[A] & registers[B];
        }
        static void bani(int A, int B, int C)
        {
            registers[C] = registers[A] & B;
        }

        static void borr(int A, int B, int C)
        {
            registers[C] = registers[A] | registers[B];
        }
        static void bori(int A, int B, int C)
        {
            registers[C] = registers[A] | B;
        }

        static void setr(int A, int B, int C)
        {
            registers[C] = registers[A];
        }

        static void seti(int A, int B, int C)
        {
            registers[C] = A;
        }
        static void gtir(int A, int B, int C)
        {
            if (A > registers[B])
                registers[C] = 1;
            else
                registers[C] = 0;
        }
        static void gtri(int A, int B, int C)
        {
            if (registers[A] > B)
                registers[C] = 1;
            else
                registers[C] = 0;
        }
        static void gtrr(int A, int B, int C)
        {
            if (registers[A] > registers[B])
                registers[C] = 1;
            else
                registers[C] = 0;
        }
        static void eqir(int A, int B, int C)
        {
            if (A == registers[B])
                registers[C] = 1;
            else
                registers[C] = 0;
        }
        static void eqri(int A, int B, int C)
        {
            if (registers[A] == B)
                registers[C] = 1;
            else
                registers[C] = 0;
        }
        static void eqrr(int A, int B, int C)
        {
            if (registers[A] == registers[B])
                registers[C] = 1;
            else
                registers[C] = 0;
        }
    }
}
