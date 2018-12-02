namespace Day17
{
    class Scan
    {
        public int StartX { get; set; }
        public int EndX { get; set; }
        public int StartY { get; set; }
        public int EndY { get; set; }

        public Scan(string line)
        {
            //x=495, y=2..7
            //y=7, x=495..501
            var parser = new Parser(line);

            if (parser.TryMatch("x="))
            {
                StartX = parser.ReadNextInt();
                EndX = StartX;
                parser.Match(", y=");
                StartY = parser.ReadNextInt();
                parser.Match("..");
                EndY = parser.ReadNextInt();
            }
            else if (parser.TryMatch("y="))
            {
                StartY = parser.ReadNextInt();
                EndY = StartY;
                parser.Match(", x=");
                StartX = parser.ReadNextInt();
                parser.Match("..");
                EndX = parser.ReadNextInt();
            }
            else
            {
                throw new System.Exception("Unexpected line - " + line);
            }
        }


    }
}
