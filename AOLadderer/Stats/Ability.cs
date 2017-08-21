using System.Collections.Generic;

namespace AOLadderer.Stats
{
    public sealed class Ability : Stat
    {
        private double _treatmentTrickleMultiplier;

        private Ability(string name, double treatmentTrickleMultiplier = 0)
            : base(name)
        {
            _treatmentTrickleMultiplier = treatmentTrickleMultiplier;
            ShortName = name.Substring(0, 3);
        }

        public string ShortName { get; }

        public override int GetShinyStatIncrease(int implantQL)
            => (int)(4.7487437 + 0.2512563 * implantQL + 0.5);

        public override int GetBrightStatIncrease(int implantQL)
            => (int)(2.8492462 + 0.1507538 * implantQL + 0.5);

        public override int GetFadedStatIncrease(int implantQL)
            => (int)(1.8994975 + 0.1005025 * implantQL + 0.5);

        public double GetTreatmentTrickle(int abilityValue)
            => _treatmentTrickleMultiplier * abilityValue / 4;

        public static readonly Ability
            Agility = new Ability("Agility", 0.3),
            Intelligence = new Ability("Intelligence", 0.5),
            Psychic = new Ability("Psychic"),
            Sense = new Ability("Sense", 0.2),
            Stamina = new Ability("Stamina"),
            Strength = new Ability("Strength");

        public static readonly IReadOnlyList<Ability> Abilities = new[] { Agility, Intelligence, Psychic, Sense, Stamina, Strength };
    }
}
