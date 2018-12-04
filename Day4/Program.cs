using System;
using System.Collections.Generic;
using System.Linq;

namespace Day4
{
    class Program
    {
        static void Main(string[] args)
        {
            var shifts = System.IO.File.ReadAllLines("ShiftPattern.txt")
                                .Select(line => new Shift(line))
                                .OrderBy(s => s.EventDate);

            var minutesByGuard = MinutesSleptByGuards(shifts);

            var worstGuard = minutesByGuard
                                    .Select(k => (k.Key, k.Value.Sum()))
                                    .OrderByDescending(k => k.Item2)
                                    .Select(k => k.Key)
                                    .First();

            var part1 = SolvePartOne(minutesByGuard, worstGuard);

            var part2 = SolvePartTwo(minutesByGuard);

        }

        private static int SolvePartTwo(Dictionary<int, int[]> minutesByGuard)
        {
            var maxNumberOfMinutes = 0;
            var maxGuard = 0;
            var maxMinute = 0;
            foreach (var guard in minutesByGuard)
            {
                for (int i = 0; i < 60; i++)
                {
                    if (guard.Value[i] > maxNumberOfMinutes)
                    {
                        maxNumberOfMinutes = guard.Value[i];
                        maxGuard = guard.Key;
                        maxMinute = i;
                    }
                }
            }
            return maxMinute * maxGuard;
        }

        private static int SolvePartOne(Dictionary<int, int[]> minutesByGuard, int worstGuard)
        {
            var minutesForWorstGuard = minutesByGuard[worstGuard];
            var bestTime = 0;
            var bestMinute = 0;
            for (int i = 0; i < 60; i++)
            {
                if (minutesForWorstGuard[i] > bestTime)
                {
                    bestTime = minutesForWorstGuard[i];
                    bestMinute = i;
                }
            }

            return bestMinute * worstGuard;
        }

        private static Dictionary<int, int[]> MinutesSleptByGuards(IOrderedEnumerable<Shift> shiftsInDateOrder)
        {
            var timeAsleep = DateTime.MinValue;
            var guardNumber = -1;
            var minutesByGuard = new Dictionary<int, int[]>();
            foreach (var shift in shiftsInDateOrder)
            {
                switch (shift.EventOnShift)
                {
                    case Shift.Event.BeginsShift:
                        guardNumber = shift.Guard;
                        break;

                    case Shift.Event.FallsAsleep:
                        timeAsleep = shift.EventDate;
                        break;

                    case Shift.Event.WakesUp:

                        if (!minutesByGuard.TryGetValue(guardNumber, out var minutesForGuard))
                        {
                            minutesForGuard = new int[60];
                            minutesByGuard.Add(guardNumber, minutesForGuard);
                        }

                        for (int minute = timeAsleep.Minute; minute < shift.EventDate.Minute; minute++)
                        {
                            minutesForGuard[minute] = minutesForGuard[minute] + 1;
                        }

                        break;
                    default:
                        throw new Exception("Unknown shift event type");
                }
            }

            return minutesByGuard;
        }
    }
}
