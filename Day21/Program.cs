using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day21
{
    class Program
    {
        static readonly int[] registers = new int[6];

        static void Main(string[] args)
        {
            var allLines = ReadFile();

            DisplayFile();
            var commands = LoadCommands(out var ipRegister,  allLines);

            RunCommands(ipRegister, commands);

            DisplayRegisters();

            Console.ReadKey(true);
        }

        private static void RunCommands(int ipRegister, Action[] commands)
        {
            registers[0] = 0;
            var ipPointer = registers[ipRegister];
            var cycle = new HashSet<int>();
            while (ipPointer < commands.Length)
            {
                //var t = $"ip={ipPointer} [";
                //for (int i = 0; i < 6; i++)
                //    t += ($"{registers[i]} ");
                //t += "]";

                registers[ipRegister] = ipPointer;
                commands[ipPointer]();
                ipPointer = registers[ipRegister] + 1;

                //t += $" {line} [";
                //for (int i = 0; i < 6; i++)
                //    t += ($"{registers[i]} ");
                //t += "]";

                if (ipPointer == 29)
                {
                    if (cycle.Contains(registers[2]))
                    {
                        Console.WriteLine($"Part 1 is {cycle.First()}");
                        Console.WriteLine($"Part 2 is {cycle.Last()}");
                        Console.ReadKey(true);
                    }
                    else
                        cycle.Add(registers[2]);
                }

            }
        }

        private static Action[] LoadCommands(out int ipRegister, string[] allLines)
        {
            var commands = new Action[allLines.Count()];
            var commandNumber = 0;
            ipRegister = 0;

            foreach (var line in allLines)
            {
                var parser = new Parser(line);
                if (parser.TryMatch("#ip "))
                {
                    ipRegister = parser.ReadNextInt();
                    parser.Match(' ');
                }

                var command = parser.ReadNextWord();
                parser.Match(' ');
                var A = parser.ReadNextInt();
                parser.Match(' ');
                var B = parser.ReadNextInt();
                parser.Match(' ');
                var C = parser.ReadNextInt();

                switch (command)
                {
                    case "addr":
                        commands[commandNumber] = addr(A, B, C);
                        break;
                    case "addi":
                        commands[commandNumber] = addi(A, B, C);
                        break;
                    case "mulr":
                        commands[commandNumber] = mulr(A, B, C);
                        break;
                    case "muli":
                        commands[commandNumber] = muli(A, B, C);
                        break;
                    case "banr":
                        commands[commandNumber] = banr(A, B, C);
                        break;
                    case "bani":
                        commands[commandNumber] = bani(A, B, C);
                        break;
                    case "borr":
                        commands[commandNumber] = borr(A, B, C);
                        break;
                    case "bori":
                        commands[commandNumber] = bori(A, B, C);
                        break;
                    case "setr":
                        commands[commandNumber] = setr(A, B, C);
                        break;
                    case "seti":
                        commands[commandNumber] = seti(A, B, C);
                        break;
                    case "gtir":
                        commands[commandNumber] = gtir(A, B, C);
                        break;
                    case "gtri":
                        commands[commandNumber] = gtri(A, B, C);
                        break;
                    case "gtrr":
                        commands[commandNumber] = gtrr(A, B, C);
                        break;
                    case "eqir":
                        commands[commandNumber] = eqir(A, B, C);
                        break;
                    case "eqri":
                        commands[commandNumber] = eqri(A, B, C);
                        break;
                    case "eqrr":
                        commands[commandNumber] = eqrr(A, B, C);
                        break;
                    default:
                        break;
                }
                commandNumber++;
            }

            return commands;
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

                if (C == 5)
                {
                    switch (command)
                    {
                        case "addi":
                            if (A == 5)
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
                            if (A == 5 && B == 5)
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
                var a = (A == 5) ? ip.ToString() : AsLetter($"[{A}]");
                var b = (B == 5) ? ip.ToString() : AsLetter($"[{B}]");
                var c = (C == 5) ? "ip" : AsLetter($"[{C}]");
                var description = "";
                switch (command)
                {
                    case "addi":
                        description = $"{c} = {a} + {B}";
                        break;

                    case "bani":
                        description = $"{c} = {a} & {B}";
                        break;

                    case "bori":
                        description = $"{c} = {a} | {B}";
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

                    case "eqri":
                        description = $"if {a} == {B} then {c} = 1 else {c} = 0";
                        break;

                    case "gtrr":
                        description = $"if {a} > {b} then {c} = 1 else {c} = 0";
                        break;

                    case "gtir":
                        description = $"if {A} > {b} then {c} = 1 else {c} = 0";
                        break;

                    default:
                        break;
                }

                if (!string.IsNullOrWhiteSpace(descriptions[ip]))
                    description = descriptions[ip];

                if (string.IsNullOrWhiteSpace(labels[ip])) labels[ip] = "        ";
                var cmd = $"{ip.ToString("00")} {labels[ip]} {line}".PadRight(40);
                Console.WriteLine($"{cmd}{description}");
                ip++;
            }

            // Console.ReadKey(true);
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

        static Action addr(int A, int B, int C) => () => registers[C] = registers[A] + registers[B];

        static Action addi(int A, int B, int C)
        {
            return () => registers[C] = registers[A] + B;
        }

        static Action mulr(int A, int B, int C)
        {
            return () => registers[C] = registers[A] * registers[B];
        }

        static Action muli(int A, int B, int C)
        {
            return () => registers[C] = registers[A] * B;
        }
        static Action banr(int A, int B, int C)
        {
            return () => registers[C] = registers[A] & registers[B];
        }
        static Action bani(int A, int B, int C)
        {
            return () => registers[C] = registers[A] & B;
        }

        static Action borr(int A, int B, int C)
        {
            return () => registers[C] = registers[A] | registers[B];
        }
        static Action bori(int A, int B, int C)
        {
            return () => registers[C] = registers[A] | B;
        }

        static Action setr(int A, int B, int C)
        {
            return () => registers[C] = registers[A];
        }

        static Action seti(int A, int B, int C)
        {
            return () => registers[C] = A;
        }
        static Action gtir(int A, int B, int C)
        {
            return () =>
            {
                if (A > registers[B])
                    registers[C] = 1;
                else
                    registers[C] = 0;
            };
        }
        static Action gtri(int A, int B, int C)
        {
            return () =>
            {
                if (registers[A] > B)
                    registers[C] = 1;
                else
                    registers[C] = 0;
            };
        }
        static Action gtrr(int A, int B, int C)
        {
            return () =>
            {
                if (registers[A] > registers[B])
                    registers[C] = 1;
                else
                    registers[C] = 0;
            };
        }
        static Action eqir(int A, int B, int C)
        {
            return () =>
            {
                if (A == registers[B])
                    registers[C] = 1;
                else
                    registers[C] = 0;
            };
        }
        static Action eqri(int A, int B, int C)
        {
            return () =>
            {
                if (registers[A] == B)
                    registers[C] = 1;
                else
                    registers[C] = 0;
            };
        }
        static Action eqrr(int A, int B, int C)
        {
            return () =>
            {
                if (registers[A] == registers[B])
                    registers[C] = 1;
                else
                    registers[C] = 0;
            };
        }
    }
}
