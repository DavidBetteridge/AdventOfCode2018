namespace Day9
{
    class Circle
    {
        private int[] _values;
        private int[] _next;
        private int[] _previous;
        private int _current;
        private int _nextSlot;

        public Circle(int numberOfMarbles)
        {
            _values = new int[numberOfMarbles];
            _next = new int[numberOfMarbles];
            _previous = new int[numberOfMarbles];

            _values[0] = 0;
            _current = 0;
            _nextSlot = 1;
        }

        public void AddMarble(int marble)
        {
            _values[_nextSlot] = marble;

            var indexOfPlusOne = _next[_current];
            var indexOfPlusTwo = _next[indexOfPlusOne];

            _next[indexOfPlusOne] = _nextSlot;
            _previous[_nextSlot] = indexOfPlusOne;

            _next[_nextSlot] = indexOfPlusTwo;
            _previous[indexOfPlusTwo] = _nextSlot;

            _current = _nextSlot;
            _nextSlot++;
        }

        public override string ToString()
        {
            var answer = "(" + _values[_current] + ")";
            var index = _next[_current];

            while (index != _current)
            {
                answer += " " + _values[index];
                index = _next[index];
            }

            return answer;

        }

        internal int RemoveMarbles(int marble)
        {
            for (int i = 0; i < 6; i++)
                _current = _previous[_current];

            var sevenBack = _previous[_current];

            _next[_previous[sevenBack]] = _next[sevenBack];
            _previous[_next[sevenBack]] = _previous[sevenBack];

            return marble + _values[sevenBack];
        }

    }
}
