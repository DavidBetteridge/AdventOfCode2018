using System;

namespace Day4
{
    class Shift
    {

        public Shift(string line)
        {
            var parser = new Parser(line);

            parser.Match('[');
            Year = parser.ReadNextInt();
            parser.Match('-');
            Month = parser.ReadNextInt();
            parser.Match('-');
            Day = parser.ReadNextInt();
            parser.Match(' ');
            Hour = parser.ReadNextInt();
            parser.Match(':');
            Minute = parser.ReadNextInt();
            parser.Match(']');
            parser.Match(' ');

            EventDate = new DateTime(Year, Month, Day, Hour, Minute, 0);

            if (parser.TryMatch("Guard #"))
            {
                Guard = parser.ReadNextInt();
                EventOnShift = Event.BeginsShift;
            }
            else if (parser.TryMatch("falls asleep"))
            {
                EventOnShift = Event.FallsAsleep;
            }
            else if (parser.TryMatch("wakes up"))
            {
                EventOnShift = Event.WakesUp;
            }
            else
            {
                throw new Exception("Could not parse the line " + line);
            }
        }

        public int Minute { get; }
        public int Hour { get; }
        public int Day { get; }
        public int Month { get; }
        public int Year { get; }
        public int Guard { get; set; }

        public Event EventOnShift { get; }
        public DateTime EventDate { get; }

        public enum Event
        {
            BeginsShift = 0,
            FallsAsleep = 1,
            WakesUp = 2
        }
    }
}
