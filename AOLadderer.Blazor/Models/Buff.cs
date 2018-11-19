using AOLadderer.Stats;

namespace AOLadderer.Blazor.Models
{
    public class Buff
    {
        public static readonly Buff Composites = new Buff("Composites", allAbilities: 12, treatment: 20);
        public static readonly Buff Composites_Pay2Win = new Buff("Composites (Pay2Win)", allAbilities: 20, treatment: 35);
        public static readonly Buff SurgeryClinicEffect = new Buff("Surgery Clinic Effect", treatment: 100);
        public static readonly Buff SuperiorFirstAid = new Buff("Superior First Aid", treatment: 80);
        public static readonly Buff EnhancedSenses = new Buff("Enhanced Senses", sense: 15);
        public static readonly Buff FelineGrace = new Buff("Feline Grace", agility: 25);
        public static readonly Buff EssenceOfBehemoth = new Buff("Essence of Behemoth", strength: 27, stamina: 27);
        public static readonly Buff Gauntlet = new Buff("Gauntlet", allAbilities: 10);
        public static readonly Buff ExtruderBar = new Buff("Extruder Bar", allAbilities: 2);
        public static readonly Buff Explosifs = new Buff("Explosifs", allAbilities: 20, treatmentDoesntTrickle: true);
        public static readonly Buff IronCircle = new Buff("Iron Circle", strength: 20, stamina: 20);
        public static readonly Buff ProdigiousStrength = new Buff("Prodigious Strength", strength: 40);
        public static readonly Buff NeuronalStimulator = new Buff("Neuronal Stimulator", intelligence: 23, psychic: 23);

        private Buff(
            string name,
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
            this.Name = name;
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

        public string Name { get; private set; }
        public int Strength { get; private set; }
        public int Agility { get; private set; }
        public int Stamina { get; private set; }
        public int Intelligence { get; private set; }
        public int Sense { get; private set; }
        public int Psychic { get; private set; }
        public double Treatment { get; private set; }

        public override string ToString() => Name;
    }
}
