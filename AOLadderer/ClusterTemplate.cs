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
            RaisesAbility = Stat is Ability;
            RaisesTreatment = Stat == Skill.Treatment;
            RaisesLadderStat = Stat.IsLadderStat;
            Ability = stat as Ability;
        }

        public Stat Stat { get; }
        public ClusterGrade ClusterGrade { get; }
        public ImplantSlot ImplantSlot { get; }
        public bool RaisesAbility { get; }
        public bool RaisesTreatment { get; }
        public bool RaisesLadderStat { get; }
        public Ability Ability { get; }

        public abstract int GetStatIncrease(int implantQL);

        public override string ToString()
            => $"{Stat.Name} Cluster - {ClusterGrade} ({ImplantSlot.Name})";
    }
}
