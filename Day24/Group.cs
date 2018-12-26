using System.Collections.Generic;

namespace Day24
{
    class Group
    {
        public int Units { get; set; }
        public int HitPoints { get; set; }
        public int AttackDamage { get; set; }
        public string DamageType { get; set; }
        public int Initiative { get; set; }

        public List<DefenceTypeAndStrength> DefenceTypes { get; set; }

        //Each group also has an effective power: the number of units in that group multiplied by their attack damage.
        public int EffectivePower => Units * AttackDamage;

        public bool UnderAttack { get; internal set; }
        public Group Target { get; internal set; }

        //956 units each with 7120 hit points(weak to bludgeoning, slashing) with an attack that does 71 radiation damage at initiative 7
        //immune to radiation; weak to fire, cold
        public Group(string line)
        {
            DefenceTypes = new List<DefenceTypeAndStrength>();

            var parser = new Parser(line);
            Units = parser.ReadNextInt();
            parser.Match(" units each with ");
            HitPoints = parser.ReadNextInt();
            parser.Match(" hit points ");
            if (parser.TryMatch("("))
            {
                //weak to bludgeoning, slashing
                //weak to cold; immune to bludgeoning, slashing
                var attackTypes = parser.ReadToNext(')');
                var types = attackTypes.Split("; ", System.StringSplitOptions.None);
                foreach (var type in types)
                {
                    var firstSpace = type.IndexOf(' ');
                    var strength = type.Substring(0, firstSpace);
                    var secondSpace = type.IndexOf(' ', firstSpace + 1);
                    var attacks = type.Substring(secondSpace + 1).Split(", ", System.StringSplitOptions.None);
                    foreach (var attack in attacks)
                    {
                        DefenceTypes.Add(new DefenceTypeAndStrength()
                        {
                            Strength = strength,
                            AttackType = attack
                        });
                    }
                }
                parser.Match(") ");
            }
            parser.Match("with an attack that does ");
            AttackDamage = parser.ReadNextInt();
            parser.Match(" ");
            DamageType = parser.ReadNextWord();
            parser.Match(" damage at initiative ");
            Initiative = parser.ReadNextInt();
        }

    }

    class DefenceTypeAndStrength
    {
        public string AttackType { get; set; }
        public string Strength { get; set; }
    }
}