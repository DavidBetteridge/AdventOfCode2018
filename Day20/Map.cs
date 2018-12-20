using System.Linq;

namespace Day20
{
    class Map
    {
        private int _x;
        private int _y;
        private readonly char[,] knownLocations;
        private readonly int _maxN;
        private readonly int _maxS;
        private readonly int _maxW;
        private readonly int _maxE;

        public (int x, int y) Location
        {
            get { return (_x, _y); }
            set { _x = value.x; _y = value.y; }
        }

        public Map(string path)
        {
            _maxN = path.Count(c => c == 'N');
            _maxS = path.Count(c => c == 'S');
            _maxW = path.Count(c => c == 'W');
            _maxE = path.Count(c => c == 'E');

            var Xrange = _maxE + _maxW;
            var Yrange = _maxN + _maxS;

            knownLocations = new char[Xrange, Yrange];

            _x = 0;
            _y = 0;

            MakeRoom(0, 0);
            knownLocations[0 + _maxW, 0 + _maxS] = 'X';
        }

        private void MakeRoom(int x, int y)
        {
            knownLocations[_x + _maxW, _y + _maxS] = '.';

            if (knownLocations[_x + _maxW, _y + _maxS - 1] == '.')
                knownLocations[_x + _maxW, _y + _maxS - 1] = '?';

            if (knownLocations[_x + _maxW, _y + _maxS + 1] == '.')
                knownLocations[_x + _maxW, _y + _maxS + 1] = '?';

            if (knownLocations[_x + _maxW - 1, _y + _maxS] == '.')
                knownLocations[_x + _maxW - 1, _y + _maxS] = '?';

            if (knownLocations[_x + _maxW + 1, _y + _maxS] == '.')
                knownLocations[_x + _maxW + 1, _y + _maxS] = '?';
        }

        internal char[,] AsGrid()
        {
            var minX = int.MaxValue;
            var minY = int.MaxValue;
            var maxX = int.MinValue;
            var maxY = int.MinValue;
            for (int y = 0; y <= knownLocations.GetUpperBound(1); y++)
            {
                for (int x = 0; x <= knownLocations.GetUpperBound(0); x++)
                {
                    if (knownLocations[x, y] != '\0')
                    {
                        if (x < minX) minX = x;
                        if (y < minY) minY = y;
                        if (x > maxX) maxX = x;
                        if (y > maxY) maxY = y;
                    }
                }
            }
            minX--;
            minY--;
            maxX++;
            maxY++;

            var grid = new char[maxX - minX + 1, maxY - minY + 1];
            for (int y = 0; y <= grid.GetUpperBound(1); y++)
            {
                for (int x = 0; x <= grid.GetUpperBound(0); x++)
                {
                    grid[x, y] = knownLocations[x + minX, y + minY];
                    if (grid[x, y] == '\0') grid[x, y] = '#';
                }
            }
            return grid;

        }


        public void WalkN()
        {
            knownLocations[_x + _maxW, _y + _maxS - 1] = '-';

            _y -= 2;
            MakeRoom(_x, _y);
        }
        public void WalkS()
        {
            knownLocations[_x + _maxW, _y + _maxS + 1] = '_';

            _y += 2;
            MakeRoom(_x, _y);
        }

        public void WalkW()
        {
            knownLocations[_x - 1 + _maxW, _y + _maxS] = '|';

            _x -= 2;
            MakeRoom(_x, _y);
        }

        public void WalkE()
        {
            knownLocations[_x + 1 + _maxW, _y + _maxS] = '|';

            _x += 2;
            MakeRoom(_x, _y);

        }
    }
}
