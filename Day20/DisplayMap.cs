using System;
using System.IO;

namespace Day20
{
    class DisplayMap
    {
        internal void Draw(TextWriter textWriter, Map map)
        {
            var grid = map.AsGrid();

            for (int y = 0; y <= grid.GetUpperBound(1); y++)
            {
                var line = "";
                for (int x = 0; x <= grid.GetUpperBound(0); x++)
                {
                    line += grid[x, y];
                }
                Console.WriteLine(line);
            }

        }
    }
}
