using System;

namespace Day17
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

        internal void Match(string text)
        {
            if (_line.Length >= (_offset + text.Length) &&
                _line.Substring(_offset, text.Length) == text)
            {
                _offset += text.Length;
                return;
            }

            throw new Exception($"Could not find {text}");
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

        internal bool TryMatch(string text)
        {
            if (_line.Length >= (_offset + text.Length) && 
                _line.Substring(_offset, text.Length) == text)
            {
                _offset += text.Length;
                return true;
            }

            return false;
        }
    }
}
