using System;
using System.Collections.Generic;
using System.Linq;

namespace Day15
{
    class Program
    {
        static void Main(string[] args)
        {
            Part2();
        }

        static void Part2()
        {
            var elfPower = 4;
            while (true)
            {
                Console.WriteLine("Trying " + elfPower);

                var board = Transpose(System.IO.File.ReadLines("Input.txt").Select(a => a.ToCharArray()).ToArray());

                var units = new List<Unit>();
                for (int y = 0; y < board.GetUpperBound(1); y++)
                {
                    for (int x = 0; x < board.GetUpperBound(0); x++)
                    {
                        if (board[x, y] == 'G' || board[x, y] == 'E')
                        {
                            units.Add(new Unit()
                            {
                                AttackPower = (board[x, y] == 'E') ? elfPower : 3,
                                HitPoints = 200,
                                IsAlive = true,
                                IsElf = (board[x, y] == 'E'),
                                X = x,
                                Y = y
                            });
                        }
                    }
                }

                var round = 0;
                while (true)
                {
                    foreach (var unit in units.OrderBy(u => u.Y).ThenBy(u => u.X))
                    {
                        unit.LastRoundPlayed = round;
                        if (unit.IsAlive)
                        {
                            if (InRange(board, unit))
                            {
                                Attack(board, units, unit);
                            }
                            else
                            {
                                board = MoveUnit(board, unit);
                                Attack(board, units, unit);
                            }

                            if (AnElfIsDead(units)) break;

                            if (IsFinished(units))
                            {
                                Console.WriteLine("Elfs won using a power of " + elfPower);

                                if (units.Any(u => u.IsAlive && u.LastRoundPlayed != round))
                                {
                                    //Round not complete
                                }
                                else
                                {
                                    // Round complete
                                    round++;
                                }

                                Console.WriteLine("Finished after round " + round + " score was " + Score(units, round));
                                Display(board, units);
                                System.Console.ReadKey();
                            }

                        }
                    }
                    round++;

                    if (AnElfIsDead(units)) break;
                }

                elfPower++;
            }
        }
        static void Part1()
        {
            var board = Transpose(System.IO.File.ReadLines("Input.txt").Select(a => a.ToCharArray()).ToArray());

            var units = new List<Unit>();
            for (int y = 0; y < board.GetUpperBound(1); y++)
            {
                for (int x = 0; x < board.GetUpperBound(0); x++)
                {
                    if (board[x, y] == 'G' || board[x, y] == 'E')
                    {
                        units.Add(new Unit()
                        {
                            AttackPower = 3,
                            HitPoints = 200,
                            IsAlive = true,
                            IsElf = (board[x, y] == 'E'),
                            X = x,
                            Y = y
                        });
                    }
                }
            }
            Display(board, units);

            var round = 0;
            while (true)
            {
                foreach (var unit in units.OrderBy(u => u.Y).ThenBy(u => u.X))
                {
                    unit.LastRoundPlayed = round;
                    if (unit.IsAlive)
                    {
                        if (InRange(board, unit))
                        {
                            Attack(board, units, unit);
                        }
                        else
                        {
                            board = MoveUnit(board, unit);
                            Attack(board, units, unit);
                        }

                        if (IsFinished(units))
                        {
                            if (units.Any(u => u.IsAlive && u.LastRoundPlayed != round))
                            {
                                //Round not complete
                            }
                            else
                            {
                                // Round complete
                                round++;
                            }

                            Console.WriteLine("Finished after round " + round + " score was " + Score(units, round));
                            Display(board, units);
                            System.Console.ReadKey();
                        }
                    }
                }

                round++;

                if (round == 28)
                {
                    Console.WriteLine("");
                    Console.WriteLine("After Round " + round);
                    Display(board, units);
                }
            }
            System.Console.ReadKey();
        }

        private static bool AnElfIsDead(List<Unit> units)
        {
            return units.Any(u => !u.IsAlive && u.IsElf);
        }

        private static bool IsFinished(List<Unit> units)
        {
            return !units.Any(u => u.IsAlive && u.IsElf) ||
                   !units.Any(u => u.IsAlive && !u.IsElf);
        }

        private static int Score(List<Unit> units, int completeRounds)
        {
            return completeRounds * units.Where(u => u.IsAlive).Sum(u => u.HitPoints);
        }

        private static void Attack(char[,] board, List<Unit> units, Unit unit)
        {
            var unitToAttack = UnitToAttack(board, units, unit);
            if (unitToAttack == null) return;  //nothing to do

            unitToAttack.HitPoints = unitToAttack.HitPoints - unit.AttackPower;
            if (unitToAttack.HitPoints <= 0)
            {
                unitToAttack.IsAlive = false;
                board[unitToAttack.X, unitToAttack.Y] = '.';
            }
        }

        private static Unit UnitAtPosition(List<Unit> units, int x, int y)
        {
            return units.SingleOrDefault(unit => unit.IsAlive &&
                                                 unit.X == x &&
                                                 unit.Y == y);
        }

        public static Unit UnitToAttack(char[,] board, List<Unit> units, Unit unit)
        {
            var left = int.MaxValue;
            if (unit.X > 0)
            {
                var enemy = UnitAtPosition(units, unit.X - 1, unit.Y);
                if (enemy != null && enemy.IsElf != unit.IsElf)
                {
                    left = enemy.HitPoints;
                }
            }

            var up = int.MaxValue;
            if (unit.Y > 0)
            {
                var enemy = UnitAtPosition(units, unit.X, unit.Y - 1);
                if (enemy != null && enemy.IsElf != unit.IsElf)
                {
                    up = enemy.HitPoints;
                }
            }

            var right = int.MaxValue;
            if (unit.X < board.GetUpperBound(0))
            {
                var enemy = UnitAtPosition(units, unit.X + 1, unit.Y);
                if (enemy != null && enemy.IsElf != unit.IsElf)
                {
                    right = enemy.HitPoints;
                }
            }

            var down = int.MaxValue;
            if (unit.Y < board.GetUpperBound(1))
            {
                var enemy = UnitAtPosition(units, unit.X, unit.Y + 1);
                if (enemy != null && enemy.IsElf != unit.IsElf)
                {
                    down = enemy.HitPoints;
                }
            }

            var lowest = Math.Min(Math.Min(Math.Min(left, up), right), down);
            if (lowest == int.MaxValue) return null;

            if (up == lowest) return UnitAtPosition(units, unit.X, unit.Y - 1);
            if (left == lowest) return UnitAtPosition(units, unit.X - 1, unit.Y);
            if (right == lowest) return UnitAtPosition(units, unit.X + 1, unit.Y);
            if (down == lowest) return UnitAtPosition(units, unit.X, unit.Y + 1);

            throw new Exception("no lowest");
        }


        private static bool InRange(char[,] board, Unit unit)
        {
            var enemy = unit.IsElf ? 'G' : 'E';

            return (ReadCell(board, unit.X + 1, unit.Y) == enemy ||
                    ReadCell(board, unit.X - 1, unit.Y) == enemy ||
                    ReadCell(board, unit.X, unit.Y + 1) == enemy ||
                    ReadCell(board, unit.X, unit.Y - 1) == enemy);
        }
        private static char[,] MoveUnit(char[,] board, Unit unit)
        {
            var unitIs = board[unit.X, unit.Y];
            var enemy = unit.IsElf ? 'G' : 'E';
            var inRange = InRange(board, enemy);
            var reachable = Reachable(inRange, unit.X, unit.Y);
            var distances = Distances(board, unit.X, unit.Y);
            var nearest = Nearest(reachable, distances);

            var chosen = Choose(nearest, unit.X, unit.Y);
            if (chosen.x == -1 && chosen.y == -1) return board;  //no where to move

            var withChosen = (char[,])board.Clone();
            withChosen[chosen.x, chosen.y] = '+';

            var shortestPath = Distances(board, chosen.x, chosen.y);

            var step = FindStep(shortestPath, unit.X, unit.Y);
            var withStep = (char[,])board.Clone();
            withStep[unit.X, unit.Y] = '.';
            withStep[step.X, step.Y] = unitIs;
            unit.X = step.X;
            unit.Y = step.Y;

            return withStep;
        }

        private static (int X, int Y) FindStep(int[,] shortestPath, int x, int y)
        {
            var left = (x > 0) ? shortestPath[x - 1, y] : int.MaxValue;
            var up = (y > 0) ? shortestPath[x, y - 1] : int.MaxValue;
            var right = (x < shortestPath.GetUpperBound(0)) ? shortestPath[x + 1, y] : int.MaxValue;
            var down = (y < shortestPath.GetUpperBound(1)) ? shortestPath[x, y + 1] : int.MaxValue;
            var lowest = Math.Min(Math.Min(Math.Min(left, up), right), down);

            if (up == lowest) return (x, y - 1);
            if (left == lowest) return (x - 1, y);
            if (right == lowest) return (x + 1, y);
            if (down == lowest) return (x, y + 1);

            throw new Exception("no lowest");
        }

        private static (int x, int y) Choose(char[,] board, int startX, int startY)
        {
            for (int y = 0; y < board.GetUpperBound(1); y++)
                for (int x = 0; x < board.GetUpperBound(0); x++)
                    if (board[x, y] == '!') return (x, y);

            return (-1, -1);
        }

        private static char[,] Nearest(char[,] board, int[,] distances)
        {
            // Find the @s with the lowest score in distances.
            var lowestScore = int.MaxValue;
            var lowests = new List<(int x, int y)>();
            for (int y = 0; y < board.GetUpperBound(1); y++)
            {
                for (int x = 0; x < board.GetUpperBound(0); x++)
                {
                    if (board[x, y] == '@')
                    {
                        if (distances[x, y] < lowestScore)
                        {
                            lowestScore = distances[x, y];
                            lowests = new List<(int x, int y)> { (x, y) };
                        }
                        else if (distances[x, y] == lowestScore)
                        {
                            lowests.Add((x, y));
                        }
                    }
                }
            }

            var result = (char[,])board.Clone();
            foreach (var (x, y) in lowests)
            {
                result[x, y] = '!';
            }
            return result;
        }

        private static int[,] Distances(char[,] board, int startX, int startY)
        {
            var result = new int[board.GetUpperBound(0), board.GetUpperBound(1)];
            for (int y = 0; y < board.GetUpperBound(1); y++)
                for (int x = 0; x < board.GetUpperBound(0); x++)
                    result[x, y] = int.MaxValue;

            var toExamine = new Stack<(int x, int y)>();

            result[startX, startY] = 0;
            toExamine.Push((startX, startY));

            while (toExamine.Any())
            {
                var (x, y) = toExamine.Pop();

                if (ReadCell(board, x - 1, y) == '.')
                {
                    if (result[x, y] + 1 < result[x - 1, y])
                    {
                        result[x - 1, y] = result[x, y] + 1;
                        toExamine.Push((x - 1, y));
                    }
                }

                if (ReadCell(board, x + 1, y) == '.')
                {
                    if (result[x, y] + 1 < result[x + 1, y])
                    {
                        result[x + 1, y] = result[x, y] + 1;
                        toExamine.Push((x + 1, y));
                    }
                }

                if (ReadCell(board, x, y - 1) == '.')
                {
                    if (result[x, y] + 1 < result[x, y - 1])
                    {
                        result[x, y - 1] = result[x, y] + 1;
                        toExamine.Push((x, y - 1));
                    }
                }

                if (ReadCell(board, x, y + 1) == '.')
                {
                    if (result[x, y] + 1 < result[x, y + 1])
                    {
                        result[x, y + 1] = result[x, y] + 1;
                        toExamine.Push((x, y + 1));
                    }
                }

            }

            return result;
        }

        private static char[,] Reachable(char[,] board, int startX, int startY)
        {
            var result = (char[,])board.Clone();

            var seen = new HashSet<string>();
            var toExamine = new Stack<(int x, int y)>();

            toExamine.Push((startX, startY));

            while (toExamine.Any())
            {
                var (x, y) = toExamine.Pop();
                if (result[x, y] == '?')
                {
                    result[x, y] = '@';
                }
                seen.Add($"{x},{y}");

                if (ReadCell(result, x - 1, y) == '.' && !seen.Contains($"{x - 1},{y}")) toExamine.Push((x - 1, y));
                if (ReadCell(result, x, y - 1) == '.' && !seen.Contains($"{x},{y - 1}")) toExamine.Push((x, y - 1));
                if (ReadCell(result, x + 1, y) == '.' && !seen.Contains($"{x + 1},{y}")) toExamine.Push((x + 1, y));
                if (ReadCell(result, x, y + 1) == '.' && !seen.Contains($"{x},{y + 1}")) toExamine.Push((x, y + 1));

                if (ReadCell(result, x - 1, y) == '?') { result[x - 1, y] = '@'; toExamine.Push((x - 1, y)); }
                if (ReadCell(result, x, y - 1) == '?') { result[x, y - 1] = '@'; toExamine.Push((x, y - 1)); }
                if (ReadCell(result, x + 1, y) == '?') { result[x + 1, y] = '@'; toExamine.Push((x + 1, y)); }
                if (ReadCell(result, x, y + 1) == '?') { result[x, y + 1] = '@'; toExamine.Push((x, y + 1)); }
            }


            return result;
        }

        private static char[,] InRange(char[,] board, char enemy)
        {
            var result = (char[,])board.Clone();

            for (int y = 0; y <= board.GetUpperBound(1); y++)
            {
                for (int x = 0; x <= board.GetUpperBound(0); x++)
                {
                    if (result[x, y] == '.')
                    {
                        if (ReadCell(result, x + 1, y) == enemy ||
                            ReadCell(result, x - 1, y) == enemy ||
                            ReadCell(result, x, y + 1) == enemy ||
                            ReadCell(result, x, y - 1) == enemy)
                        {
                            result[x, y] = '?';
                        }
                    }
                }
            }

            return result;
        }

        private static char ReadCell(char[,] board, int x, int y)
        {
            if (x >= 0 && y >= 0 && x <= board.GetUpperBound(0) && y <= board.GetUpperBound(1))
            {
                return board[x, y];
            }
            return ' ';
        }

        private static void Display(char[,] board, List<Unit> units)
        {
            for (int y = 0; y <= board.GetUpperBound(1); y++)
            {
                var line = "";
                var postFix = "";
                for (int x = 0; x <= board.GetUpperBound(0); x++)
                {
                    line += board[x, y];

                    var unit = UnitAtPosition(units, x, y);
                    if (unit != null)
                        postFix = postFix + $"({(unit.IsElf ? 'E' : 'G')}{unit.HitPoints})";

                }
                System.Console.WriteLine(line + " " + postFix);
            }
        }

        private static char[,] Transpose(char[][] matrix)
        {
            int w = matrix.Count();
            int h = matrix[0].Length;

            var result = new char[h, w];

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    result[j, i] = matrix[i][j];
                }
            }

            return result;
        }
    }
}
