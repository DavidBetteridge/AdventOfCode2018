using System;
using System.Collections.Generic;

namespace Day20
{
    class PathWalker
    {
        private readonly string _path;
        private readonly Map _map;
        private int _offset;

        public PathWalker(string path, Map map)
        {
            _path = path;
            _map = map;
            _offset = 1;
        }

        internal void WalkPath()
        {
            if (_offset >= _path.Length - 1) return;

            switch (_path[_offset])
            {
                case '$':
                    return;

                case '(':
                    WalkOptions();
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
                    throw new Exception($"Unknown symbol {_path[_offset]}");
            }
            _offset++;
            WalkPath();
        }

        private void WalkOptions()
        {
            var options = new List<string>();
            var openCount = 1;
            var currentLine = "";

            _offset++;
            while (openCount > 0)
            {
                switch (_path[_offset])
                {
                    case '(':
                        openCount++;
                        currentLine += _path[_offset];
                        break;

                    case ')':
                        openCount--;
                        if (openCount > 0)
                            currentLine += _path[_offset];
                        break;

                    case '|':
                        if (openCount == 1)
                        {
                            options.Add(currentLine);
                            currentLine = "";
                        }
                        else
                            currentLine += _path[_offset];
                        break;

                    default:
                        currentLine += _path[_offset];
                        break;
                }
                _offset++;
            }
            options.Add(currentLine);

            var location = _map.Location;
            foreach (var option in options)
            {
                _map.Location = location;
                var remainder = _path.Substring(_offset);
                var path = "^" + option + remainder;
                var pathWalker = new PathWalker(path, _map);
                pathWalker.WalkPath();
            }
         //   Console.ReadKey(true);
        }
    }
}
