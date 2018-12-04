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
                                .Select(line => new Shift(line));

            var shiftsInDateOrder = shifts.OrderBy(s => s.EventDate);

            var amountOfSleep = new Dictionary<int, TimeSpan>();

            var guardNumber = -1;
            var timeAsleep = DateTime.MaxValue;
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
                        var timeSlept = shift.EventDate - timeAsleep;

                        if (amountOfSleep.TryGetValue(guardNumber, out var current))
                        {
                            amountOfSleep[guardNumber] = timeSlept + current;
                        }
                        else
                        {
                            amountOfSleep[guardNumber] = timeSlept;
                        }

                        break;
                    default:
                        throw new Exception("Unknown shift event type");
                }
            }


            var maxSleep = 0D;
            var worstGuard = -1;
            foreach (var item in amountOfSleep)
            {
                if (item.Value.TotalSeconds > maxSleep)
                {
                    maxSleep = item.Value.TotalSeconds;
                    worstGuard = item.Key;
                }
            }


            var minutes = new int[60];

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
                        if (guardNumber == worstGuard)
                        {
                            for (int minute = timeAsleep.Minute; minute < shift.EventDate.Minute; minute++)
                            {
                                minutes[minute] = minutes[minute] + 1;
                            }
                        }

                        break;
                    default:
                        throw new Exception("Unknown shift event type");
                }
            }

            var bestTime = 0;
            var bestMinute = 0;
            for (int i = 0; i < 60; i++)
            {
                if (minutes[i] > bestTime)
                {
                    bestTime = minutes[i];
                    bestMinute = i;
                }
            }

            var answer = bestMinute * worstGuard;

            ////////////////////////////////////////////////////////////////////////////


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


            var part2 = maxMinute * maxGuard;

        }
    }
}
