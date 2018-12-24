using System.Collections.Generic;

namespace Day24
{
    class Group
    {
        public int Units { get; set; }
        public int HitPoints { get; set; }
        public int Damage { get; set; }
        public string DamageType { get; set; }
        public int Initiative { get; set; }
        
        public List<AttackTypeAndStrength> AttackTypes { get; set; }

        //956 units each with 7120 hit points(weak to bludgeoning, slashing) with an attack that does 71 radiation damage at initiative 7
        //immune to radiation; weak to fire, cold
        public Group(string line)
        {
            var parser = new Parser(line);
            Units = parser.ReadNextInt();
            parser.Match(" units each with ");
            HitPoints = parser.ReadNextInt();
            parser.Match(" hit points ");
            if (parser.TryMatch("("))
            {
                //weak to bludgeoning, slashing
                //weak to cold; immune to bludgeoning, slashing
                AttackTypes = new List<AttackTypeAndStrength>();
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
                        AttackTypes.Add(new AttackTypeAndStrength()
                        {
                            Strength = strength,
                            AttackType = attack
                        });
                    }
                }
                parser.Match(") ");
            }
            parser.Match("with an attack that does ");
            Damage = parser.ReadNextInt();
            parser.Match(" ");
            DamageType = parser.ReadNextWord();
            parser.Match(" damage at initiative ");
            Initiative = parser.ReadNextInt();
        }

    }

    class AttackTypeAndStrength
    {
        public string AttackType { get; set; }
        public string Strength { get; set; }
    }
}
