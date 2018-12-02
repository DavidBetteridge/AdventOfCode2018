using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            Part2();
            registers[0]=1;
            var ipRegister = 0;
            var ipPointer = 0;
            var counter = 0;
            var allLines = ReadFile();

            DisplayFile();

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
                Console.WriteLine(t);

                counter++;
                //  if (counter % 100 == 1)
                //     DisplayRegisters();
                Console.ReadKey(true);
            }

            DisplayRegisters();

            Console.ReadKey(true);
        }

        private static void Part2()
        {
            var C = 1;
            var A = 0;
            var E = 1;
            while (C <= 914)
            {
                if (C * E == 914) A += C;
                E = E + 1;
                if (E > 914)
                {
                    C = C + 1;
                    E = 1;
                }
            }
            Console.WriteLine("Part 2 is " + A);
        }

        private static void DisplayFile()
        {
            var allLines = File.ReadAllLines("Input.txt");
            var ip = 0;
            var nextLabel = 0;
            var labels = new string[allLines.Length - 1];
            var descriptions = new string[allLines.Length - 1];
            foreach (var line in allLines.Skip(1))
            {
                var parser = new Parser(line);
                var command = parser.ReadNextWord();
                parser.Match(' ');
                var A = parser.ReadNextInt();
                parser.Match(' ');
                var B = parser.ReadNextInt();
                parser.Match(' ');
                var C = parser.ReadNextInt();

                if (C == 3)
                {
                    switch (command)
                    {
                        case "addi":
                            if (A == 3)
                            {
                                if (string.IsNullOrWhiteSpace(labels[ip + B + 1]))
                                {
                                    labels[ip + B + 1] = $"Label{nextLabel}: ";
                                    nextLabel++;
                                }
                                descriptions[ip] = $"Goto " + labels[ip + B + 1];
                            }
                            break;

                        case "seti":
                            if (string.IsNullOrWhiteSpace(labels[A + 1]))
                            {
                                labels[A + 1] = $"Label{nextLabel}: ";
                                nextLabel++;
                            }
                            descriptions[ip] = $"Goto " + labels[A + 1];
                            break;

                        case "mulr":
                            if (A == 3 && B == 3)
                            {
                                var target = (ip * ip) + 1;
                                if (target < labels.Length)
                                {
                                    labels[ip + B + 1] = $"Label{nextLabel}: ";
                                    descriptions[ip] = $"Goto Label {nextLabel}";
                                    nextLabel++;
                                }
                                else
                                {
                                    descriptions[ip] = $"Goto EXIT";
                                }
                            }
                            break;

                        default:
                            break;
                    }
                }

                ip++;
            }

            string AsLetter(string what)
            {
                switch (what)
                {
                    case "[0]": return "A";
                    case "[1]": return "B";
                    case "[2]": return "C";
                    case "[3]": return "D";
                    case "[4]": return "E";
                    case "[5]": return "F";
                    default:
                        return "";
                }
            }

            ip = 0;
            foreach (var line in allLines.Skip(1))
            {
                var parser = new Parser(line);
                var command = parser.ReadNextWord();
                parser.Match(' ');
                var A = parser.ReadNextInt();
                parser.Match(' ');
                var B = parser.ReadNextInt();
                parser.Match(' ');
                var C = parser.ReadNextInt();
                var a = (A == 3) ? ip.ToString() : AsLetter($"[{A}]");
                var b = (B == 3) ? ip.ToString() : AsLetter($"[{B}]");
                var c = (C == 3) ? "ip" : AsLetter($"[{C}]");
                var description = "";
                switch (command)
                {
                    case "addi":
                        description = $"{c} = {a} + {B}";
                        break;

                    case "addr":
                        description = $"{c} = {a} + {b}";
                        break;

                    case "seti":
                        description = $"{c} = {A}";
                        break;

                    case "setr":
                        description = $"{c} = {a}";
                        break;

                    case "mulr":
                        description = $"{c} = {a} * {b}";
                        break;

                    case "muli":
                        description = $"{c} = {a} * {B}";
                        break;

                    case "eqrr":
                        description = $"if {a} == {b} then {c} = 1 else {c} = 0";
                        break;

                    case "gtrr":
                        description = $"if {a} > {b} then {c} = 1 else {c} = 0";
                        break;

                    default:
                        break;
                }

                if (!string.IsNullOrWhiteSpace(descriptions[ip]))
                    description = descriptions[ip];

                if (string.IsNullOrWhiteSpace(labels[ip])) labels[ip] = "        ";
                Console.WriteLine($"{ip.ToString("00")} {labels[ip]} {line}\t\t{description}");
                ip++;
            }

            Console.ReadKey(true);
        }

        private static void DisplayRegisters()
        {
            var l = "";
            for (int i = 0; i < 6; i++)
                l += ($"{registers[i]} ");
            Console.WriteLine(l);
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
