using System.Collections.Generic;

namespace AOLadderer.Stats
{
    public sealed class ArmorClass : Stat
    {
        private ArmorClass(string name)
            : base(name)
        { }

        public override int GetShinyStatIncrease(int implantQL)
            => (int)(5.5025126 + 2.4974874 * implantQL + 0.5);

        public override int GetBrightStatIncrease(int implantQL)
            => (int)(2.4974874 + 1.5025126 * implantQL + 0.5);

        public override int GetFadedStatIncrease(int implantQL)
            => (int)(2.0000000 + 1.0000000 * implantQL + 0.5);

        public static readonly ArmorClass
            ChemicalAC = new ArmorClass("Chemical AC"),
            ColdAC = new ArmorClass("Cold AC"),
            DiseaseAC = new ArmorClass("Disease AC"),
            EnergyAC = new ArmorClass("Energy AC"),
            FireAC = new ArmorClass("Fire AC"),
            ImpProjAC = new ArmorClass("Imp/Proj AC"),
            MeleeMaAC = new ArmorClass("Melee/Ma AC"),
            RadiationAC = new ArmorClass("Radiation AC");

        public static readonly IReadOnlyList<ArmorClass> ArmorClasses = new[] { ChemicalAC, ColdAC, DiseaseAC, EnergyAC, FireAC, ImpProjAC, MeleeMaAC, RadiationAC };
    }
}
