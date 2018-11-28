using AOLadderer.Stats;

namespace AOLadderer.Blazor.Models
{
    public class Buff
    {
        public static readonly Buff Composites = new Buff(allAbilities: 12, treatment: 20);
        public static readonly Buff Composites_Pay2Win = new Buff(allAbilities: 20, treatment: 35);
        public static readonly Buff SurgeryClinicEffect = new Buff(treatment: 100);
        public static readonly Buff SuperiorFirstAid = new Buff(treatment: 80);
        public static readonly Buff EnhancedSenses = new Buff(sense: 15);
        public static readonly Buff FelineGrace = new Buff(agility: 25);
        public static readonly Buff EssenceOfBehemoth = new Buff(strength: 27, stamina: 27);
        public static readonly Buff Gauntlet = new Buff(allAbilities: 10);
        public static readonly Buff ExtruderBar = new Buff(allAbilities: 2);
        public static readonly Buff Explosifs = new Buff(allAbilities: 20, treatmentDoesntTrickle: true);
        public static readonly Buff IronCircle = new Buff(strength: 20, stamina: 20);
        public static readonly Buff ProdigiousStrength = new Buff(strength: 40);
        public static readonly Buff IronCircleOrProdigiousStrength = new Buff(stamina: 40, strength: 20);
        public static readonly Buff NeuronalStimulator = new Buff(intelligence: 23, psychic: 23);

        private Buff(
            int strength = 0,
            int agility = 0,
            int stamina = 0,
            int intelligence = 0,
            int sense = 0,
            int psychic = 0,
            int allAbilities = 0,
            double treatment = 0.0,
            bool treatmentDoesntTrickle = false)
        {
            this.Strength = strength + allAbilities;
            this.Agility = agility + allAbilities;
            this.Stamina = stamina + allAbilities;
            this.Intelligence = intelligence + allAbilities;
            this.Sense = sense + allAbilities;
            this.Psychic = psychic + allAbilities;

            double treatmentTrickle = Ability.Sense.GetTreatmentTrickle(sense)
                + Ability.Sense.GetTreatmentTrickle(allAbilities)
                + Ability.Agility.GetTreatmentTrickle(agility)
                + Ability.Agility.GetTreatmentTrickle(allAbilities)
                + Ability.Intelligence.GetTreatmentTrickle(intelligence)
                + Ability.Intelligence.GetTreatmentTrickle(allAbilities);

            this.Treatment = treatment + (treatmentDoesntTrickle ? 0.0 : treatmentTrickle);
        }

        public int Strength { get; private set; }
        public int Agility { get; private set; }
        public int Stamina { get; private set; }
        public int Intelligence { get; private set; }
        public int Sense { get; private set; }
        public int Psychic { get; private set; }
        public double Treatment { get; private set; }
    }
}
