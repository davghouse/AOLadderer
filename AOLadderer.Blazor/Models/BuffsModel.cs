using System.Collections.Generic;
using System.Linq;

namespace AOLadderer.Blazor.Models
{
    public enum CompositesBuff
    {
        Standard, Pay2Win
    }

    public enum StrengthAndStaminaBuff
    {
        IronCircle, ProdigiousStrength, IronCircleOrProdigiousStrength
    }

    public class BuffsModel
    {
        public CompositesBuff? Composites { get; set; }
        public bool UseSurgeryClinicEffect { get; set; }
        public bool UseSuperiorFirstAid { get; set; }
        public bool UseEnhancedSenses { get; set; }
        public bool UseFelineGrace { get; set; }
        public bool UseEssenceOfBehemoth { get; set; }
        public bool UseGauntlet { get; set; }
        public bool UseExtruderBar { get; set; }
        public bool UseExplosifs { get; set; }
        public StrengthAndStaminaBuff? StrengthAndStamina { get; set; }
        public bool UseNeuronalStimulator { get; set; }

        protected IEnumerable<Buff> Buffs
        {
            get
            {
                if (Composites == CompositesBuff.Standard) yield return Buff.Composites;
                if (Composites == CompositesBuff.Pay2Win) yield return Buff.Composites_Pay2Win;
                if (UseSurgeryClinicEffect) yield return Buff.SurgeryClinicEffect;
                if (UseSuperiorFirstAid) yield return Buff.SuperiorFirstAid;
                if (UseEnhancedSenses) yield return Buff.EnhancedSenses;
                if (UseFelineGrace) yield return Buff.FelineGrace;
                if (UseEssenceOfBehemoth) yield return Buff.EssenceOfBehemoth;
                if (UseGauntlet) yield return Buff.Gauntlet;
                if (UseExtruderBar) yield return Buff.ExtruderBar;
                if (UseExplosifs) yield return Buff.Explosifs;
                if (StrengthAndStamina == StrengthAndStaminaBuff.IronCircle) yield return Buff.IronCircle;
                if (StrengthAndStamina == StrengthAndStaminaBuff.ProdigiousStrength) yield return Buff.ProdigiousStrength;

                if (StrengthAndStamina == StrengthAndStaminaBuff.IronCircleOrProdigiousStrength)
                {
                    yield return Buff.IronCircle;
                    yield return Buff.ProdigiousStrength;
                }

                if (UseNeuronalStimulator) yield return Buff.NeuronalStimulator;
            }
        }

        public int Strength => Buffs.Sum(b => b.Strength);
        public int Agility => Buffs.Sum(b => b.Agility);
        public int Stamina => Buffs.Sum(b => b.Stamina);
        public int Intelligence => Buffs.Sum(b => b.Intelligence);
        public int Sense => Buffs.Sum(b => b.Sense);
        public int Psychic => Buffs.Sum(b => b.Psychic);
        public double Treatment => Buffs.Sum(b => b.Treatment);
    }
}
