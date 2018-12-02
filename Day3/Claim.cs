namespace Day3
{
    public class Claim
    {
        public Claim(string line)
        {
            var parser = new Parser(line);
            parser.Match('#');
            Id = parser.ReadNextInt();
            parser.Match(' ');
            parser.Match('@');
            parser.Match(' ');
            Left = parser.ReadNextInt();
            parser.Match(',');
            Top = parser.ReadNextInt();
            parser.Match(':');
            parser.Match(' ');
            Width = parser.ReadNextInt();
            parser.Match('x');
            Height = parser.ReadNextInt();
        }

        public int Id { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
