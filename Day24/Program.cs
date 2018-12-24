using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day24
{
    class Program
    {
        static void Main(string[] args)
        {
            var allLines = File.ReadAllLines("Input.txt");
            var immuneArmy = new List<Group>();
            var infectionArmy = new List<Group>();

            var lineNumber = 1;
            while (!string.IsNullOrWhiteSpace(allLines[lineNumber]))
            {
                immuneArmy.Add(new Group(allLines[lineNumber]));
                lineNumber++;
            }

            lineNumber += 2;
            while (lineNumber < allLines.Length && !string.IsNullOrWhiteSpace(allLines[lineNumber]))
            {
                infectionArmy.Add(new Group(allLines[lineNumber]));
                lineNumber++;
            }

        }
    }
}


