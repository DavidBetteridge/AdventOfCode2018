using System;

namespace Day3
{
    class Parser
    {
        private readonly string _line;
        private int _offset;

        public Parser(string line)
        {
            _line = line;
            _offset = 0;
        }

        public void Match(char what)
        {
            if (what != _line[_offset]) throw new Exception($"found {_line[_offset]} instead of {what} not found");
            _offset++;
        }

        internal int ReadNextInt()
        {
            var numberAsText = "";
            while (_offset < _line.Length && char.IsDigit(_line[_offset]))
            {
                numberAsText += _line[_offset];
                _offset++;
            }

            if (numberAsText == "") throw new Exception("No number found");

            return int.Parse(numberAsText);
        }
    }
}
