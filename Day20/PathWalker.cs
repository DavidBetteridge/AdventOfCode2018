using System;
using System.Collections.Generic;

namespace Day20
{
    class PathWalker
    {
        private readonly Map _map;

        public PathWalker(Map map)
        {
            _map = map;
        }

        internal void WalkPath(string path, int offset)
        {
            if (offset >= path.Length - 1) return;

            switch (path[offset])
            {
                case '$':
                    return;

                case '(':
                    WalkOptions(path, offset);
                    return;

                case 'N':
                    _map.WalkN();
                    break;

                case 'S':
                    _map.WalkS();
                    break;

                case 'E':
                    _map.WalkE();
                    break;

                case 'W':
                    _map.WalkW();
                    break;

                default:
                    throw new Exception($"Unknown symbol {path[offset]}");
            }
            WalkPath(path, offset + 1);
        }

        private void WalkOptions(string path, int offset)
        {
            var options = new List<string>();
            var openCount = 1;
            var currentLine = "";

            offset++;
            while (openCount > 0)
            {
                switch (path[offset])
                {
                    case '(':
                        openCount++;
                        currentLine += path[offset];
                        break;

                    case ')':
                        openCount--;
                        if (openCount > 0)
                            currentLine += path[offset];
                        break;

                    case '|':
                        if (openCount == 1)
                        {
                            options.Add(currentLine);
                            currentLine = "";
                        }
                        else
                            currentLine += path[offset];
                        break;

                    default:
                        currentLine += path[offset];
                        break;
                }
                offset++;
            }
            options.Add(currentLine);

            var location = _map.Location;
            foreach (var option in options)
            {
                _map.Location = location;
                var remainder = path.Substring(offset);
                var path2 = option + remainder;
                WalkPath(path2, 0);
            }
            //   Console.ReadKey(true);
        }

        //private static unsafe string JoinInternal(ReadOnlySpan<char> first, ReadOnlySpan<char> second)
        //{
        //    fixed (char* f = &MemoryMarshal.GetReference(first), s = &MemoryMarshal.GetReference(second))
        //    {
        //        return string.Create(
        //            first.Length + second.Length,
        //            (First: (IntPtr)f, FirstLength: first.Length, Second: (IntPtr)s, SecondLength: second.Length),
        //            (destination, state) =>
        //            {
        //                new Span<char>((char*)state.First, state.FirstLength).CopyTo(destination);
        //                new Span<char>((char*)state.Second, state.SecondLength).CopyTo(destination.Slice(state.FirstLength));
        //            });
        //    }
        //}

    }
}
