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

            var rootNode = new Node()
            {
                Path = '\0',
                Options = new List<Node>()
            };

            BuildTree(rootNode, path);


            //SplitNodeIfNeeded(rootNode);

            //DrawGraph(rootNode, "");

            var map = new Map(path);
            WalkMap(map, rootNode);

            var displayMap = new DisplayMap();
            displayMap.Draw(Console.Out, map);

            Console.ReadKey(true);
        }

        private static void BuildTree(Node rootNode, string path)
        {
            var nextNode = rootNode;
            var index = 0;
            while (index < path.Length && path[index] != '(' && path[index] != '$')
            {
                var child = nextNode.Options.SingleOrDefault(n => n.Path == path[index]);
                if (child == null)
                {
                    child = new Node() { Path = path[index], Options = new List<Node>() };
                    nextNode.Options.Add(child);
                }
                nextNode = child;
                index++;
            }

            (var remainingText, var options) = ReadOptions(path.Substring(index));
            foreach (var option in options)
            {
                BuildTree(nextNode, option + remainingText);
            }

        }

        //private static void DrawGraph(Node rootNode, string indent)
        //{
        //    //     Console.WriteLine(indent + rootNode.Path);
        //    foreach (var option in rootNode.Options)
        //    {
        //        DrawGraph(option, rootNode.Path);
        //    }
        //}

        //private static void SquashGraph(Node rootNode)
        //{
        //    for (int maxDepth = 2; maxDepth < 10; maxDepth++)
        //    {
        //        var prefix = rootNode.Path;
        //        DropALevel(rootNode, rootNode, prefix, 1, maxDepth);
        //    }
        //}

        //private static void DropALevel(Node rootNode, Node parentNode, string prefix, int depth, int maxDepth)
        //{
        //    if (depth > maxDepth) return;

        //    foreach (var option in parentNode.Options)
        //    {
        //        var path = prefix + option.Path;
        //        var duplicateNode = FindDuplicateNode(rootNode, option, rootNode.Path, path);
        //        if (duplicateNode != null)
        //        {

        //        }
        //        DropALevel(rootNode, option, prefix, depth + 1, maxDepth);
        //    }
        //}


        //private static Node FindDuplicateNode(Node parentNode, Node ignoreThisNode, string prefix, string path)
        //{
        //    foreach (var option in parentNode.Options)
        //    {
        //        if (option == ignoreThisNode) return null;  //No point going any further
        //        var localPath = prefix + option.Path;
        //        if (localPath == path) return option;
        //        if (localPath.Length < path.Length)
        //        {
        //            var duplicate = FindDuplicateNode(option, ignoreThisNode, localPath, path);
        //            if (duplicate != null) return duplicate;
        //        }
        //    }
        //    return null;
        //}

        private static void WalkMap(Map map, Node rootNode)
        {
            switch (rootNode.Path)
            {
                case '\0':
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
                    throw new Exception("Unknown step type " + rootNode.Path);
            }

            var location = map.Location;
            foreach (var option in rootNode.Options)
            {
                WalkMap(map, option);
                map.Location = location;
            }

        }

        //private static void SplitNodeIfNeeded(Node node)
        //{
        //    var optionStart = node.Path.IndexOf('(');
        //    if (optionStart > -1)
        //    {
        //        (var remainingText, var options) = ReadOptions(node.Path.Substring(optionStart));
        //        node.Path = node.Path.Substring(0, optionStart);

        //        var followingNode = new Node()
        //        {
        //            Path = remainingText,
        //            Options = new List<Node>()
        //        };

        //        var nextNode = node.Options.SingleOrDefault();
        //        if (nextNode != null)
        //            followingNode.Options.Add(nextNode);

        //        if (followingNode.Path == "")
        //        {
        //            followingNode = nextNode;
        //        }


        //        foreach (var option in options)
        //        {
        //            if (string.IsNullOrWhiteSpace(option))
        //            {
        //                node.Options.Add(followingNode);
        //            }
        //            else
        //            {
        //                var optionNode = new Node()
        //                {
        //                    Path = option,
        //                    Options = new List<Node>() { followingNode }
        //                };
        //                SplitNodeIfNeeded(optionNode);
        //                node.Options.Add(optionNode);
        //            }
        //        }

        //        SplitNodeIfNeeded(followingNode);

        //    }


        //}


        private static (string remainingText, List<string> options) ReadOptions(string fullPath)
        {
            if (fullPath == "$") return ("", new List<string>());

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
