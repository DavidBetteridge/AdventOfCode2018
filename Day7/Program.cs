using System.Collections.Generic;
using System.Linq;

namespace Day7
{
    class Program
    {
        static void Main(string[] args)
        {
            var steps = System.IO.File.ReadAllLines("Steps.txt")
                                            .Select((line, index) => new Step(line));

            const int StepOverHead = 60;
            const int NumberOfWorkers = 5;
            var workers = new Worker[NumberOfWorkers] { new Worker(), new Worker(), new Worker(), new Worker(), new Worker() };

            //const int StepOverHead = 0;
            //const int NumberOfWorkers = 2;
            //var workers = new Worker[NumberOfWorkers] { new Worker(), new Worker() };

            var allSteps = new HashSet<string>();
            var depends = new Dictionary<string, List<string>>();
            foreach (var step in steps)
            {
                allSteps.Add(step.FromStep);
                if (!depends.TryGetValue(step.ToStep, out var dependsOn))
                {
                    dependsOn = new List<string>();
                    depends.Add(step.ToStep, dependsOn);
                }
                dependsOn.Add(step.FromStep);
            }

            var firsts = allSteps.Where(s => !depends.ContainsKey(s));
            foreach (var first in firsts)
            {
                var dependsOn = new List<string>();
                depends.Add(first, dependsOn);
            }

            System.Console.WriteLine($"Second  Worker 1     Worker 2    Done");
            var done = "";
            var timeTaken = 0;
            while (depends.Any() || workers.Any(w => w.TimeRemaining > 0))
            {

                // Another second has passed.  Decrease the TimeRemaining on all workers.
                // If they have now reached zero,  then they are free,  and the task is complete.
                foreach (var worker in workers.Where(w => w.TimeRemaining > 0))
                {
                    worker.TimeRemaining--;
                    if (worker.TimeRemaining == 0)
                    {
                        foreach (var item in depends.Values)
                        {
                            if (item.Contains(worker.CurrentActivity))
                            {
                                item.Remove(worker.CurrentActivity);
                            }
                        }

                        done += worker.CurrentActivity;

                        worker.CurrentActivity = "";
                    }
                }

                // Is there anywork for our workers todo?
                foreach (var worker in workers.Where(w => string.IsNullOrWhiteSpace(w.CurrentActivity)))
                {
                    var nextJob = depends.Where(d => !d.Value.Any()).OrderBy(d => d.Key).FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(nextJob.Key))
                    {
                        worker.CurrentActivity = nextJob.Key;
                        worker.TimeRemaining = StepOverHead + (nextJob.Key[0] - 'A') + 1;
                        depends.Remove(nextJob.Key);
                    }
                }

                //System.Console.WriteLine($"{timeTaken}  {workers[0].CurrentActivity}    {workers[1].CurrentActivity}    {done}");
                System.Console.WriteLine($"{timeTaken}  {workers[0].CurrentActivity}    {workers[1].CurrentActivity}    {workers[2].CurrentActivity}    {workers[3].CurrentActivity}    {workers[4].CurrentActivity}    {done}");

                timeTaken++;

            }
            System.Console.Write(timeTaken - 1);

        }
        void Part1()
        {
            var steps = System.IO.File.ReadAllLines("Steps.txt")
                                            .Select((line, index) => new Step(line));

            var allSteps = new HashSet<string>();
            var depends = new Dictionary<string, List<string>>();
            foreach (var step in steps)
            {
                allSteps.Add(step.FromStep);
                if (!depends.TryGetValue(step.ToStep, out var dependsOn))
                {
                    dependsOn = new List<string>();
                    depends.Add(step.ToStep, dependsOn);
                }
                dependsOn.Add(step.FromStep);
            }

            var firsts = allSteps.Where(s => !depends.ContainsKey(s));
            foreach (var first in firsts)
            {
                var dependsOn = new List<string>();
                depends.Add(first, dependsOn);
            }

            while (depends.Any())
            {
                var possible = depends.Where(d => !d.Value.Any()).OrderBy(d => d.Key).First().Key;

                foreach (var item in depends.Values)
                {
                    if (item.Contains(possible))
                    {
                        item.Remove(possible);
                    }
                }

                depends.Remove(possible);
                System.Console.Write(possible);
            }

        }
    }

    internal class Worker
    {
        public string CurrentActivity { get; set; }
        public int TimeRemaining { get; set; }
    }

    internal class Step
    {
        public string FromStep { get; set; }
        public string ToStep { get; set; }
        public Step(string line)
        {
            line = line.Replace(" must be finished before step ", "");
            line = line.Replace(" can begin.", "");
            line = line.Replace("Step ", "");

            FromStep = line[0].ToString();
            ToStep = line[1].ToString();
        }
    }
}
