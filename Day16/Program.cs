using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day16
{
    class Program
    {
        static readonly int[] registers = new int[4];

        static readonly Action<int, int, int>[] instructions = new Action<int, int, int>[16]
                { addr, addi, mulr, muli, banr, bani, borr, bori, setr, seti, gtir, gtri, gtrr, eqir, eqri, eqrr };

        static void Main(string[] args)
        {
            var opCodesCouldBe = new Dictionary<int, List<string>>();
            for (int opCode = 0; opCode < 16; opCode++)
                opCodesCouldBe.Add(opCode, new List<string> { "addr", "addi", "mulr", "muli", "banr", "bani", "borr", "bori", "setr", "seti", "gtir", "gtri", "gtrr", "eqir", "eqri", "eqrr" });

            var answer = 0;
            var allLines = System.IO.File.ReadAllLines("Input.txt");
            for (int lineNumber = 0; lineNumber < allLines.Length; lineNumber = lineNumber + 4)
            {
                var before = allLines[lineNumber].Replace("Before: [", "").Replace("]", "").Replace(" ", "").Split(',').Select(a => int.Parse(a)).ToArray();
                var command = allLines[lineNumber + 1];

                var parser = new Parser(command);

                var opcode = parser.ReadNextInt();
                parser.Match(' ');

                var A = parser.ReadNextInt();
                parser.Match(' ');

                var B = parser.ReadNextInt();
                parser.Match(' ');

                var C = parser.ReadNextInt();

                var after = allLines[lineNumber + 2].Replace("After:  [", "").Replace("]", "").Replace(" ", "").Split(',').Select(a => int.Parse(a)).ToArray();

                if (FindPossibleInstructions(before, opcode, A, B, C, after, opCodesCouldBe) >= 3)
                    answer++;
            }
            Console.WriteLine("Part 1 is " + answer);

            var opCodeMap = new Action<int, int, int>[16];
            while (opCodesCouldBe.Any())
            {
                foreach (var instruction in instructions)
                {
                    var numberOfMatches = 0;
                    var opCode = -1;
                    foreach (var item in opCodesCouldBe)
                    {
                        if (item.Value.Contains(instruction.Method.Name))
                        {
                            numberOfMatches++;
                            opCode = item.Key;
                        }
                    }
                    if (numberOfMatches == 1)
                    {
                        Console.WriteLine($"{instruction.Method.Name} is {opCode}");
                        opCodesCouldBe.Remove(opCode);
                        opCodeMap[opCode] = instruction;
                    }
                }
            }


            for (int r = 0; r <= 3; r++)
                registers[r] = 0;

            using (var fileStream = File.OpenRead("Input Part 2.txt"))
            using (var streamReader = new StreamReader(fileStream))
            {
                String line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    var parser = new Parser(line);
                    var opcode = parser.ReadNextInt();
                    parser.Match(' ');
                    var A = parser.ReadNextInt();
                    parser.Match(' ');
                    var B = parser.ReadNextInt();
                    parser.Match(' ');
                    var C = parser.ReadNextInt();

                    opCodeMap[opcode](A, B, C);
                }
            }

            Console.WriteLine("Part 2 is " + registers[0]);
            Console.ReadKey();
        }

        static int FindPossibleInstructions(int[] registersBefore, int opCode, int A, int B, int C, int[] registersAfter, Dictionary<int, List<string>> opCodesCouldBe)
        {
            var matches = 0;
            foreach (var instruction in instructions)
            {
                for (int r = 0; r <= 3; r++)
                    registers[r] = registersBefore[r];

                instruction(A, B, C);

                var ok = true;
                for (int r = 0; r <= 3; r++)
                {
                    if (registers[r] != registersAfter[r])
                        ok = false;
                }

                if (ok)
                {
                    matches++;
                }
                else
                {
                    if (opCodesCouldBe[opCode].Contains(instruction.Method.Name))
                        opCodesCouldBe[opCode].Remove(instruction.Method.Name);
                }
            }

            return matches;
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
