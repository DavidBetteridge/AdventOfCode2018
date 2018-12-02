using System;
using System.Linq;

namespace Day8
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = System.IO.File.ReadAllText("Input.txt");
            var parser = new Parser(input);

            var rootNode = ReadNode(parser);

            Console.WriteLine($"Part1 - {Part1(rootNode)}");
            Console.WriteLine($"Part2 - {Part2(rootNode)}");
            Console.ReadKey();
        }

        private static int Part1(Node input)
        {
            var sumOfChildren = input.Children.Select(c => Part1(c)).Sum();
            return input.MetaData.Sum() + sumOfChildren;
        }

        private static int Part2(Node input)
        {
            if (input.NumberOfChildNodes == 0)
                return input.MetaData.Sum();

            return input.MetaData
                        .Where(childNumber => childNumber > 0 && childNumber <= input.NumberOfChildNodes)
                        .Select(childNumber => input.Children[childNumber - 1])
                        .Sum(child => Part2(child));
        }

        private static Node ReadNode(Parser parser)
        {
            var node = new Node();
            node.NumberOfChildNodes = parser.ReadNextInt();
            parser.Match(' ');
            node.NumberOfMetaDataEntries = parser.ReadNextInt();
            parser.Match(' ');

            node.Children = new Node[node.NumberOfChildNodes];
            node.MetaData = new int[node.NumberOfMetaDataEntries];

            for (int i = 0; i < node.NumberOfChildNodes; i++)
            {
                node.Children[i] = ReadNode(parser);
            }

            for (int i = 0; i < node.NumberOfMetaDataEntries; i++)
            {
                node.MetaData[i] = parser.ReadNextInt();
                parser.TryMatch(" ");
            }

            return node;
        }



    }

    class Node
    {
        public int NumberOfChildNodes { get; set; }
        public int NumberOfMetaDataEntries { get; set; }
        public Node[] Children { get; set; }
        public int[] MetaData { get; set; }
    }
}
