using AOLadderer.Helpers;
using AOLadderer.Stats;
using System.Collections.Generic;
using System.Linq;

namespace AOLadderer
{
    public abstract class Stat
    {
        protected Stat(string name)
        {
            Name = name;
            IsLadderStat = this is Ability || name == "Treatment";
        }

        public string Name { get; }
        public bool IsLadderStat { get; }

        public abstract int GetShinyStatIncrease(int implantQL);
        public abstract int GetBrightStatIncrease(int implantQL);
        public abstract int GetFadedStatIncrease(int implantQL);

        public override string ToString()
            => Name;

        /* Can't use fields here, otherwise if derived classes are initialized first then initializing these fields
           would cycle back to the derived classes which would return null for their specific stat collections.
           See details here: (https://codeblog.jonskeet.uk/2012/04/07/type-initializer-circular-dependencies/).
           Other static fields are fine, Stat and ImplantSlot are self-contained, then ClusterTemplates depend on those,
           then ImplantTemplates depends on all of them, so it's a DAG. But seems bad... Just use properties everywhere? */

        private static IReadOnlyList<Stat> _stats;
        public static IReadOnlyList<Stat> Stats => _stats
            ?? (_stats = Ability.Abilities
                .Cast<Stat>()
                .Concat(ArmorClass.ArmorClasses)
                .Concat(MaxHealthOrNano.MaxHealthAndNano)
                .Concat(Skill.Skills)
                .ToArray());

        private static IReadOnlyList<Stat> _ladderStats;
        public static IReadOnlyList<Stat> LadderStats => _ladderStats
            ?? (_ladderStats = Ability.Abilities
                .Cast<Stat>()
                .Prepend(Skill.Treatment)
                .ToArray());
    }
}
