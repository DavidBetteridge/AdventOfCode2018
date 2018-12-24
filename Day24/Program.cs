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
                //Attack(immuneArmy, infectionArmy);

            }

        }
        private static void TargetSelection(List<Group> immuneArmy, List<Group> infectionArmy)
        {
            SelectGroupsToAttack(immuneArmy, infectionArmy);
            SelectGroupsToAttack(infectionArmy, immuneArmy);

        }

        private static void SelectGroupsToAttack(List<Group> attackingArmy, List<Group> infectionArmy)
        {
            foreach (var target in infectionArmy)
                target.UnderAttack = false;

            foreach (var attacking in attackingArmy.OrderByDescending(army => army.EffectivePower).OrderBy(army => army.Initiative))
            {
                var target = infectionArmy
                                          .Where(army => !army.UnderAttack)
                                          .GroupBy(defending => Damage(attacking, defending))
                                          .MaxBy(grp => grp.Key)
                                          .Select(grp => grp)
                                          .OrderByDescending(grp => grp.EffectivePower)
                                          .ThenByDescending(grp => grp.Initiative).FirstOrDefault();
                if (target == null)
                {
                    attacking.Target = null;
                }
                else
                {
                    target.UnderAttack = true;
                    attacking.Target = target;
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


