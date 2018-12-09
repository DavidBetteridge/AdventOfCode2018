using System.Collections.Generic;
using System.Linq;

namespace Day9
{
    class Circle
    {
        private readonly LinkedList<int> _marbles;
        private LinkedListNode<int> _current;
        public Circle(int maxSize)
        {
            _marbles = new LinkedList<int>();
            _current = new LinkedListNode<int>(0);
            _marbles.AddFirst(_current);
        }

        public void AddMarble(int marble)
        {
            var insertAfter = _current.Next;
            if (insertAfter == null)
                insertAfter = _marbles.Find(_marbles.First());

            var newNode = new LinkedListNode<int>(marble);
            _marbles.AddAfter(insertAfter, newNode);

            _current = newNode;
        }

        public override string ToString()
        {
            var sArray = new int[_marbles.Count];
            _marbles.CopyTo(sArray, 0);

            var display = "";
            for (int i = 0; i < _marbles.Count; i++)
            {
                var value = sArray[i];
                if (value == _current.Value)
                    display += $"({value}) ";
                else
                    display += value + " ";
            }
            return display;
        }

        internal int RemoveMarbles(int marble)
        {
            var score = marble;

            var toRemove = _current;
            for (int i = 0; i < 7; i++)
            {
                toRemove = toRemove.Previous;
                if (toRemove == null)
                {
                    toRemove = _marbles.Find(_marbles.Last());
                }
            }

            var removing = toRemove.Value;
            score += removing;

            _current = toRemove.Next;
            if (_current == null)
                _current = _marbles.Find(_marbles.First());

            _marbles.Remove(toRemove);

            return score;
        }

    }
}
