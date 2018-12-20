using System.Collections.Generic;
using System.Linq;

namespace Day20
{
    class Map
    {
        private int _x;
        private int _y;
        private readonly Dictionary<string, char> knownLocations = new Dictionary<string, char>();

        public (int x, int y) Location
        {
            get { return (_x, _y); }
            set { _x = value.x; _y = value.y; }
        }

        public Map()
        {
            _x = 0;
            _y = 0;

            MakeRoom(0, 0);
            knownLocations[MakeKey(0, 0)] = 'X';
        }

        private string MakeKey(int x, int y) => x + "," + y;


        private void MakeRoom(int x, int y)
        {
            if (!knownLocations.ContainsKey(MakeKey(_x, _y)))
                knownLocations[MakeKey(_x, _y)] = '.';

            if (!knownLocations.ContainsKey(MakeKey(_x, _y - 1)))
                knownLocations[MakeKey(_x, _y - 1)] = '?';

            if (!knownLocations.ContainsKey(MakeKey(_x, _y + 1)))
                knownLocations[MakeKey(_x, _y + 1)] = '?';

            if (!knownLocations.ContainsKey(MakeKey(_x - 1, _y)))
                knownLocations[MakeKey(_x - 1, _y)] = '?';

            if (!knownLocations.ContainsKey(MakeKey(_x + 1, _y)))
                knownLocations[MakeKey(_x + 1, _y)] = '?';
        }

        internal char[,] AsGrid()
        {
            var minX = knownLocations.Keys.Min(a => int.Parse(a.Split(',')[0]));
            var maxX = knownLocations.Keys.Max(a => int.Parse(a.Split(',')[0]));
            var minY = knownLocations.Keys.Min(a => int.Parse(a.Split(',')[1]));
            var maxY = knownLocations.Keys.Max(a => int.Parse(a.Split(',')[1]));

            var grid = new char[maxX - minX + 1, maxY - minY + 1];

            for (int y = 0; y <= grid.GetUpperBound(1); y++)
                for (int x = 0; x <= grid.GetUpperBound(0); x++)
                    grid[x, y] = '#';

            foreach (var item in knownLocations)
            {
                var X = int.Parse(item.Key.Split(',')[0]);
                var Y = int.Parse(item.Key.Split(',')[1]);
                grid[X - minX, Y - minY] = item.Value == '?' ? '#' : item.Value;
            }

            return grid;

        }


        public void WalkN()
        {
            knownLocations[MakeKey(_x, _y - 1)] = '-';

            _y -= 2;
            MakeRoom(_x, _y);
        }
        public void WalkS()
        {
            knownLocations[MakeKey(_x, _y + 1)] = '_';

            _y += 2;
            MakeRoom(_x, _y);
        }

        public void WalkW()
        {
            knownLocations[MakeKey(_x - 1, _y)] = '|';

            _x -= 2;
            MakeRoom(_x, _y);
        }

        public void WalkE()
        {
            knownLocations[MakeKey(_x + 1, _y)] = '|';

            _x += 2;
            MakeRoom(_x, _y);

        }
    }
}
