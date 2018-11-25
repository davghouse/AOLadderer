using System;
using System.Collections.Generic;
using System.Linq;

namespace AOLadderer.Blazor.Models
{
    public enum CompositesBuff
    {
        None, Standard, Pay2Win
    }

    public enum StrengthAndStaminaBuff
    {
        None, IronCircle, ProdigiousStrength, IronCircleOrProdigiousStrength
    }

    public class BuffsModel : IUrlTokenSerializable
    {
        public CompositesBuff Composites { get; set; }
        public bool UseSurgeryClinicEffect { get; set; }
        public bool UseSuperiorFirstAid { get; set; }
        public bool UseEnhancedSenses { get; set; }
        public bool UseFelineGrace { get; set; }
        public bool UseEssenceOfBehemoth { get; set; }
        public bool UseGauntlet { get; set; }
        public bool UseExtruderBar { get; set; }
        public bool UseExplosifs { get; set; }
        public StrengthAndStaminaBuff StrengthAndStamina { get; set; }
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

        public void UrlTokenDeserialize(Queue<object> data)
        {
            Composites = (CompositesBuff)Convert.ToInt32(data.Dequeue());
            UseSurgeryClinicEffect = Convert.ToInt32(data.Dequeue()) == 1;
            UseSuperiorFirstAid = Convert.ToInt32(data.Dequeue()) == 1;
            UseEnhancedSenses = Convert.ToInt32(data.Dequeue()) == 1;
            UseFelineGrace = Convert.ToInt32(data.Dequeue()) == 1;
            UseEssenceOfBehemoth = Convert.ToInt32(data.Dequeue()) == 1;
            UseGauntlet = Convert.ToInt32(data.Dequeue()) == 1;
            UseExtruderBar = Convert.ToInt32(data.Dequeue()) == 1;
            UseExplosifs = Convert.ToInt32(data.Dequeue()) == 1;
            StrengthAndStamina = (StrengthAndStaminaBuff)Convert.ToInt32(data.Dequeue());
            UseNeuronalStimulator = Convert.ToInt32(data.Dequeue()) == 1;
        }

        public void UrlTokenSerialize(Queue<object> data)
        {
            data.Enqueue(Composites);
            data.Enqueue(UseSurgeryClinicEffect ? 1 : 0);
            data.Enqueue(UseSuperiorFirstAid ? 1 : 0);
            data.Enqueue(UseEnhancedSenses ? 1 : 0);
            data.Enqueue(UseFelineGrace ? 1 : 0);
            data.Enqueue(UseEssenceOfBehemoth ? 1 : 0);
            data.Enqueue(UseGauntlet ? 1 : 0);
            data.Enqueue(UseExtruderBar ? 1 : 0);
            data.Enqueue(UseExplosifs ? 1 : 0);
            data.Enqueue(StrengthAndStamina);
            data.Enqueue(UseNeuronalStimulator ? 1 : 0);
        }
    }
}
