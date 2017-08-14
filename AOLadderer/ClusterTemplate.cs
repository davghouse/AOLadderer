using AOLadderer.Stats;

namespace AOLadderer
{
    public abstract class ClusterTemplate
    {
        protected ClusterTemplate(Stat stat, ClusterGrade clusterGrade, ImplantSlot implantSlot)
        {
            Stat = stat;
            ClusterGrade = clusterGrade;
            ImplantSlot = implantSlot;

            // Performance optimizations: store some commonly needed data.
            RaisesAbility = Stat is Ability;
            RaisesTreatment = Stat == Skill.Treatment;
            RaisesLadderStat = Stat.IsLadderStat;
            Ability = stat as Ability;
            ArmorClass = stat as ArmorClass;
            MaxHealthOrNano = stat as MaxHealthOrNano;
            Skill = stat as Skill;
        }

        public Stat Stat { get; }
        public ClusterGrade ClusterGrade { get; }
        public ImplantSlot ImplantSlot { get; }
        public bool RaisesAbility { get; }
        public bool RaisesTreatment { get; }
        public bool RaisesLadderStat { get; }
        public Ability Ability { get; }
        public ArmorClass ArmorClass { get; }
        public MaxHealthOrNano MaxHealthOrNano { get; }
        public Skill Skill { get; }

        public abstract int GetStatIncrease(int implantQL);

        public override string ToString()
            => $"{Stat.Name} Cluster - {ClusterGrade} ({ImplantSlot.Name})";
    }
}
