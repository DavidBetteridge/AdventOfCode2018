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
            ParseFile(out var immuneArmy, out var infectionArmy);

            while (immuneArmy.Any() && infectionArmy.Any())
            {
                TargetSelection(immuneArmy, infectionArmy);
                Attack(immuneArmy, infectionArmy);
                immuneArmy = RemoveDeadGroups(immuneArmy);
                infectionArmy = RemoveDeadGroups(infectionArmy);
            }

            if (immuneArmy.Any())
                Console.WriteLine($"Part 1 is {immuneArmy.Sum(grp=>grp.Units)}");

            if (infectionArmy.Any())
                Console.WriteLine($"Part 1 is {infectionArmy.Sum(grp => grp.Units)}");

            //14294 too low
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

            foreach (var attacking in attackingArmy.OrderByDescending(army => army.EffectivePower).OrderBy(army => army.Initiative))
            {
                attacking.Target = null;
                var possibleTargets = defendingArmy.Where(army => !army.UnderAttack);
                if (possibleTargets.Any())
                {
                    var target = possibleTargets
                                              .GroupBy(defending => Damage(attacking, defending))
                                              .MaxBy(grp => grp.Key)
                                              .Select(grp => grp)
                                              .OrderByDescending(grp => grp.EffectivePower)
                                              .ThenByDescending(grp => grp.Initiative).FirstOrDefault();
                    if (target != null)
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


