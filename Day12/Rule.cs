namespace Day12
{
    class Rule
    {
        public char L1 { get; set; }
        public char L2 { get; set; }
        public char Node { get; set; }
        public char R1 { get; set; }
        public char R2 { get; set; }
        public char Produces { get; set; }

        public Rule(string line)
        {
            L2 = line[0];
            L1 = line[1];
            Node = line[2];
            R1 = line[3];
            R2 = line[4];
            Produces = line[9];
        }

        internal bool Matches(char v1, char v2, char v3, char v4, char v5)
        {
            return v1 == L2 &&
                   v2 == L1 &&
                   v3 == Node &&
                   v4 == R1 &&
                   v5 == R2;
        }
    }
}
