using System;
using System.Collections.Generic;
using System.Linq;

namespace Day24
{
    static class Extensions
    {

        public static T MaxBy<T>(this IEnumerable<T> source, Func<T, int> action)
        {
            var bestMatch = source.FirstOrDefault();
            var best = action(bestMatch);

            foreach (var item in source)
            {
                var score = action(item);
                if (score > best)
                {
                    best = score;
                    bestMatch = item;
                }
            }
            return bestMatch;
        }
    }
}
