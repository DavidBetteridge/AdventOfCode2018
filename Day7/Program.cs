using System.Collections.Generic;
using System.Linq;

namespace Day7
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine($"Part 1 -> {Part1()}");
            System.Console.WriteLine($"Part 2 -> {Part2()}");
            System.Console.ReadKey();
        }

        static string Part1()
        {
            var dependencies = WorkoutDependencies("steps.txt");

            var answer = "";
            while (dependencies.Any())
            {
                var possible = dependencies.Where(d => !d.Value.Any()).OrderBy(d => d.Key).First().Key;

                foreach (var item in dependencies.Values)
                {
                    if (item.Contains(possible))
                        item.Remove(possible);
                }

                dependencies.Remove(possible);
                answer += possible;
            }

            return answer;
        }

        static int Part2()
        {
            const int StepOverHead = 60;
            const int NumberOfWorkers = 5;
            var workers = new Worker[NumberOfWorkers] { new Worker(), new Worker(), new Worker(), new Worker(), new Worker() };

            var dependencies = WorkoutDependencies("steps.txt");

            var timeTaken = 0;
            while (dependencies.Any() || workers.Any(w => w.TimeRemaining > 0))
            {
                foreach (var worker in workers.Where(w => w.TimeRemaining > 0))
                {
                    worker.TimeRemaining--;
                    if (worker.TimeRemaining == 0)
                    {
                        foreach (var item in dependencies.Values)
                        {
                            if (item.Contains(worker.CurrentActivity))
                            {
                                item.Remove(worker.CurrentActivity);
                            }
                        }
                        worker.CurrentActivity = "";
                    }
                }

                foreach (var worker in workers.Where(w => string.IsNullOrWhiteSpace(w.CurrentActivity)))
                {
                    var nextJob = dependencies.Where(d => !d.Value.Any()).OrderBy(d => d.Key).FirstOrDefault();
                    if (string.IsNullOrWhiteSpace(nextJob.Key))
                    {
                        break;  //no point looking at the other workers if there is no more work to do.
                    }
                    else
                    {
                        worker.CurrentActivity = nextJob.Key;
                        worker.TimeRemaining = StepOverHead + (nextJob.Key[0] - 'A') + 1;
                        dependencies.Remove(nextJob.Key);
                    }
                }
                timeTaken++;
            }

            return timeTaken - 1;
        }

        private static Dictionary<string, List<string>> WorkoutDependencies(string filename)
        {
            var steps = System.IO.File.ReadAllLines(filename)
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

                if (!depends.ContainsKey(step.FromStep))
                    depends.Add(step.FromStep, new List<string>());
            }
            return depends;
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
