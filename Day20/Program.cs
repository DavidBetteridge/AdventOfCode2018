using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day20
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = File.ReadAllText("Input.txt");
            path = path.Replace("|)", ")");

            var rootNode = new Node()
            {
                Path = path,
                Options = new List<Node>()
            };
            SplitNodeIfNeeded(rootNode);

            var map = new Map(path);
            WalkMap(map, rootNode);

            Console.WriteLine($"Part 1 is {map.Part1()}");
            Console.WriteLine($"Part 2 is {map.Part2()}");


            var displayMap = new DisplayMap();
            displayMap.Draw(Console.Out, map);

            Console.ReadKey(true);
        }

        private static void WalkMap(Map map, Node rootNode)
        {
            foreach (var step in rootNode.Path)
            {
                switch (step)
                {
                    case '^':
                    case '$':
                        break;
                    case 'N':
                        map.WalkN();
                        break;
                    case 'W':
                        map.WalkW();
                        break;
                    case 'S':
                        map.WalkS();
                        break;
                    case 'E':
                        map.WalkE();
                        break;
                    default:
                        throw new Exception("Unknown step type " + step);
                }
            }

            var location = map.Location;
            foreach (var option in rootNode.Options)
            {
                WalkMap(map, option);
                map.Location = location;
            }

        }

        private static void SplitNodeIfNeeded(Node node)
        {
            var optionStart = node.Path.IndexOf('(');
            if (optionStart > -1)
            {
                (var remainingText, var options) = ReadOptions(node.Path.Substring(optionStart));
                node.Path = node.Path.Substring(0, optionStart);

                var followingNode = new Node()
                {
                    Path = remainingText,
                    Options = new List<Node>()
                };

                var nextNode = node.Options.SingleOrDefault();
                if (nextNode != null)
                    followingNode.Options.Add(nextNode);

                if (followingNode.Path == "")
                {
                    followingNode = nextNode;
                }


                foreach (var option in options)
                {
                    var optionNode = new Node()
                    {
                        Path = option,
                        Options = new List<Node>() { followingNode }
                    };
                    SplitNodeIfNeeded(optionNode);
                    node.Options.Add(optionNode);
                }


                SplitNodeIfNeeded(followingNode);

            }


        }


        private static (string remainingText, List<string> options) ReadOptions(string fullPath)
        {
            var result = new List<Node>();
            var options = new List<string>();
            var openCount = 1;
            var currentLine = "";

            var offset = 1;
            while (openCount > 0)
            {
                switch (fullPath[offset])
                {
                    case '(':
                        openCount++;
                        currentLine += fullPath[offset];
                        break;

                    case ')':
                        openCount--;
                        if (openCount > 0)
                            currentLine += fullPath[offset];
                        break;

                    case '|':
                        if (openCount == 1)
                        {
                            options.Add(currentLine);
                            currentLine = "";
                        }
                        else
                            currentLine += fullPath[offset];
                        break;

                    default:
                        currentLine += fullPath[offset];
                        break;
                }
                offset++;
            }
            options.Add(currentLine);

            return (fullPath.Substring(offset), options);
        }
    }
}