using System;
using System.Collections.Generic;
using System.IO;

namespace Day5
{
    class Program
    {
        static void Main(string[] args)
        {
            Part1();

            var lowest = int.MaxValue;
            for (char c = 'A'; c <= 'Z'; c++)
            {
                lowest = Math.Min(lowest, Part2(c));
            }
            var part2 = lowest;
        }

        private static void Part1()
        {
            var stack = new Stack<char>();
            using (var reader = new StreamReader("Polymer.txt"))
            {
                do
                {
                    var ch = (char)reader.Read();

                    if (stack.Count == 0)
                        stack.Push(ch);
                    else
                    {
                        var top = stack.Peek();
                        if (top == ch)
                        {
                            stack.Push(ch);
                        }
                        else if (char.ToLower(top) == char.ToLower(ch))
                        {
                            stack.Pop();
                        }
                        else
                        {
                            stack.Push(ch);
                        }
                    }
                } while (!reader.EndOfStream);

                var part1 = stack.Count;
            }
        }


        private static int Part2(char skip)
        {
            var stack = new Stack<char>();
            using (var reader = new StreamReader("Polymer.txt"))
            {
                do
                {
                    var ch = (char)reader.Read();
                    if (char.ToLower(skip) != char.ToLower(ch))
                    {
                        if (stack.Count == 0)
                            stack.Push(ch);
                        else
                        {
                            var top = stack.Peek();
                            if (top == ch)
                            {
                                stack.Push(ch);
                            }
                            else if (char.ToLower(top) == char.ToLower(ch))
                            {
                                stack.Pop();
                            }
                            else
                            {
                                stack.Push(ch);
                            }
                        }
                    }
                } while (!reader.EndOfStream);

                return stack.Count;
            }
        }

    }
}
