namespace Day10
{
    class Star
    {
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int SpeedX { get; set; }
        public int SpeedY { get; set; }
        public Star(string line)
        {
            //position=< 20432,  20428> velocity=<-2, -2>
            var parser = new Parser(line);
            parser.TryMatch("position=<");
            parser.TryMatch(" ");
            StartX = parser.ReadNextInt();
            parser.Match(',');
            parser.Match(' ');
            parser.TryMatch(" ");
            StartY = parser.ReadNextInt();
            parser.TryMatch("> velocity=<");
            parser.TryMatch(" ");
            SpeedX = parser.ReadNextInt();
            parser.Match(',');
            parser.Match(' ');
            parser.TryMatch(" ");
            SpeedY = parser.ReadNextInt();
        }
    }
}
