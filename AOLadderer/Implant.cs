using AOLadderer.ClusterTemplates;
using AOLadderer.Stats;
using System;
using System.Collections.Generic;

namespace AOLadderer
{
    public class Implant : IEquatable<Implant>
    {
        public Implant(ImplantTemplate implantTemplate, int implantQL)
        {
            ImplantTemplate = implantTemplate;
            QL = implantQL;
        }

        public ImplantTemplate ImplantTemplate { get; }
        public int QL { get; }
        public int AOID => ImplantTemplate.AOID;
        public ImplantSlot ImplantSlot => ImplantTemplate.ImplantSlot;
        public Ability RequiredAbility => ImplantTemplate.RequiredAbility;
        public int RequiredAbilityValue => GetRequiredAbilityValue(QL);
        public int RequiredTreatmentValue => GetRequiredTreatmentValue(QL);
        public ShinyClusterTemplate ShinyClusterTemplate => ImplantTemplate.ShinyClusterTemplate;
        public BrightClusterTemplate BrightClusterTemplate => ImplantTemplate.BrightClusterTemplate;
        public FadedClusterTemplate FadedClusterTemplate => ImplantTemplate.FadedClusterTemplate;
        public IReadOnlyList<Stat> RaisedLadderStats => ImplantTemplate.RaisedLadderStats;
        public bool RaisesLadderStats => ImplantTemplate.RaisesLadderStats;
        public IReadOnlyList<ClusterTemplate> AbilityClusterTemplates => ImplantTemplate.AbilityClusterTemplates;
        public ClusterTemplate TreatmentClusterTemplate => ImplantTemplate.TreatmentClusterTemplate;
        public Stat ShinyStat => ImplantTemplate.ShinyStat;
        public Stat BrightStat => ImplantTemplate.BrightStat;
        public Stat FadedStat => ImplantTemplate.FadedStat;
        public int? ShinyStatIncrease => ShinyClusterTemplate?.GetStatIncrease(QL);
        public int? BrightStatIncrease => BrightClusterTemplate?.GetStatIncrease(QL);
        public int? FadedStatIncrease => FadedClusterTemplate?.GetStatIncrease(QL);

        public override string ToString()
            => $"{ImplantTemplate}, QL {QL}";

        public static Implant GetImplant(ImplantSlot implantSlot, Stat shinyStat, Stat brightStat, Stat fadedStat, int implantQL)
            => new Implant(ImplantTemplate.GetImplantTemplate(implantSlot, shinyStat, brightStat, fadedStat), implantQL);

        public static int GetRequiredAbilityValue(int implantQL)
            => 4 + 2 * implantQL;

        public static int GetRequiredTreatmentValue(int implantQL)
            => (int)(6.2763819 + 4.7236181 * implantQL + 0.5);

        public static int GetMaxImplantQLForAbilityValue(int abilityValue)
            => Math.Min((abilityValue - 4) / 2, 200);

        // For example, a QL 22 implant requires 110.1959801 treatment, which gets rounded down
        // to 110. 110 treatment is then calculated here as implying a QL 21.9585106 implant.
        // That rounds down to a QL of 21, which is effectively the lower bound QL below. May be
        // a way to do this more directly but whatever. Can't just round up in general.
        public static int GetMaxImplantQLForTreatmentValue(double treatmentValue)
        {
            int effectiveTreatmentValue = (int)treatmentValue;
            int lowerBoundQL = (int)((effectiveTreatmentValue - 6.2763819) / 4.7236181);
            return effectiveTreatmentValue >= GetRequiredTreatmentValue(lowerBoundQL + 1)
                ? Math.Min(lowerBoundQL + 1, 200) : Math.Min(lowerBoundQL, 200);
        }

        public static int GetMaxImplantQL(int abilityValue, double treatmentValue)
            => Math.Min(GetMaxImplantQLForAbilityValue(abilityValue), GetMaxImplantQLForTreatmentValue(treatmentValue));

        public override bool Equals(object other)
            => Equals(other as Implant);

        public bool Equals(Implant other)
            => ImplantTemplate == other?.ImplantTemplate && QL == other?.QL;

        public override int GetHashCode()
            => ImplantTemplate.GetHashCode();
    }
}
