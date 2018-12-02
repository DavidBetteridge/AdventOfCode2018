using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day24
{
    class Program
    {
        static void Main(string[] args)
        {
            Part1();
            Part2();
            Console.ReadKey(true);

            //5216
            //15392
        }

        private static void Part1()
        {
            DoBattle(out var immuneArmy, out var infectionArmy, 0);

            if (immuneArmy.Any())
                Console.WriteLine($"Part 1 (immuneArmy) is {immuneArmy.Sum(grp => grp.Units)}");

            if (infectionArmy.Any())
                Console.WriteLine($"Part 1 (infectionArmy) is {infectionArmy.Sum(grp => grp.Units)}");
        }

        private static void Part2()
        {
            for (int boostToTry = 0; boostToTry < 1000000; boostToTry++)
            {
                Console.Write("Trying " + boostToTry);

                DoBattle(out var immuneArmy, out var infectionArmy, boostToTry);
                if (!infectionArmy.Any())
                {
                    Console.WriteLine(" win for immune");
                    Console.WriteLine($"Part 2 (immuneArmy) is {immuneArmy.Sum(grp => grp.Units)}");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine(" lose for immune");
                }
            }


        }

        private static void DoBattle(out List<Group> immuneArmy, out List<Group> infectionArmy, int amountToBoostBy)
        {
            ParseFile(out immuneArmy, out infectionArmy);

            BoostArmy(immuneArmy, amountToBoostBy);

            var unitCount = 0;
            while (immuneArmy.Any() && infectionArmy.Any())
            {
                TargetSelection(immuneArmy, infectionArmy);
                Attack(immuneArmy, infectionArmy);
                immuneArmy = RemoveDeadGroups(immuneArmy);
                infectionArmy = RemoveDeadGroups(infectionArmy);

                var newUnitCount = immuneArmy.Sum(g => g.Units) + infectionArmy.Sum(g => g.Units);
                if (newUnitCount == unitCount) break;
                unitCount = newUnitCount;
            }
        }

        private static void BoostArmy(List<Group> immuneArmy, int amountToBoostBy)
        {
            foreach (var grp in immuneArmy)
            {
                grp.AttackDamage += amountToBoostBy;
            }
        }

        private static List<Group> RemoveDeadGroups(List<Group> army)
        {
            return army.Where(grp => grp.Units > 0).ToList();
        }

        private static void Attack(List<Group> immuneArmy, List<Group> infectionArmy)
        {
            var allGroups = immuneArmy.Union(infectionArmy)
                                .Where(grp => grp.Target != null)
                                .OrderByDescending(grp => grp.Initiative);

            foreach (var attacker in allGroups)
            {
                var defender = attacker.Target;
                var damage = Damage(attacker, defender);
                if (damage >= (defender.Units * defender.HitPoints))
                {
                    //target will be destroyed
                    defender.Units = 0;
                }
                else
                {
                    var unitsDamaged = damage / defender.HitPoints;
                    defender.Units -= unitsDamaged;
                }
            }
        }

        private static void TargetSelection(List<Group> immuneArmy, List<Group> infectionArmy)
        {
            SelectGroupsToAttack(immuneArmy, infectionArmy);
            SelectGroupsToAttack(infectionArmy, immuneArmy);
        }

        private static void SelectGroupsToAttack(List<Group> attackingArmy, List<Group> defendingArmy)
        {
            foreach (var target in defendingArmy)
                target.UnderAttack = false;

            foreach (var attacking in attackingArmy.OrderByDescending(army => (army.EffectivePower, army.Initiative)))
            {
                attacking.Target = null;
                var possibleTargets = defendingArmy.Where(army => !army.UnderAttack);
                if (possibleTargets.Any())
                {
                    var target = possibleTargets
                                              .GroupBy(defending => Damage(attacking, defending))
                                              .MaxBy(grp => grp.Key)
                                              .Select(grp => grp)
                                              .OrderByDescending(grp => (grp.EffectivePower, grp.Initiative))
                                              .FirstOrDefault();
                    if (target != null && Damage(attacking, target) > 0)
                    {
                        target.UnderAttack = true;
                        attacking.Target = target;
                    }
                }
            };
        }

        private static int Damage(Group attacking, Group defending)
        {
            var attackType = attacking.DamageType;
            var attackStrength = attacking.AttackDamage;
            var power = attacking.EffectivePower;

            var defence = defending.DefenceTypes.SingleOrDefault(d => d.AttackType == attackType);
            if (defence != null)
            {
                switch (defence.Strength)
                {
                    case "weak":
                        power *= 2;
                        break;
                    case "immune":
                        power = 0;
                        break;
                    default:
                        break;
                }
            }

            return power;
        }

        private static void ParseFile(out List<Group> immuneArmy, out List<Group> infectionArmy)
        {
            var allLines = File.ReadAllLines("Input.txt");
            immuneArmy = new List<Group>();
            infectionArmy = new List<Group>();
            var lineNumber = 1;
            while (!string.IsNullOrWhiteSpace(allLines[lineNumber]))
            {
                immuneArmy.Add(new Group(allLines[lineNumber]));
                lineNumber++;
            }

            lineNumber += 2;
            while (lineNumber < allLines.Length && !string.IsNullOrWhiteSpace(allLines[lineNumber]))
            {
                infectionArmy.Add(new Group(allLines[lineNumber]));
                lineNumber++;
            }
        }


    }
}

