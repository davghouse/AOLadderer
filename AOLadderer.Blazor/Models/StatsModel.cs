using System;
using System.Collections.Generic;

namespace AOLadderer.Blazor.Models
{
    public class StatsModel : IUrlTokenSerializable
    {
        private int
            strengthBuff, agilityBuff, staminaBuff, intelligenceBuff, senseBuff, psychicBuff,
            baseStrength, baseAgility, baseStamina, baseIntelligence, baseSense, basePsychic;
        private double
            treatmentBuff, baseTreatment;

        public int Strength
        {
            get => baseStrength + strengthBuff;
            set => baseStrength = value - strengthBuff;
        }

        public int Agility
        {
            get => baseAgility + agilityBuff;
            set => baseAgility = value - agilityBuff;
        }

        public int Stamina
        {
            get => baseStamina + staminaBuff;
            set => baseStamina = value - staminaBuff;
        }

        public int Intelligence
        {
            get => baseIntelligence + intelligenceBuff;
            set => baseIntelligence = value - intelligenceBuff;
        }

        public int Sense
        {
            get => baseSense + senseBuff;
            set => baseSense = value - senseBuff;
        }

        public int Psychic
        {
            get => basePsychic + psychicBuff;
            set => basePsychic = value - psychicBuff;
        }

        public double Treatment
        {
            get => baseTreatment + treatmentBuff;
            set => baseTreatment = value - treatmentBuff;
        }

        public event Action OnBuffsApplied;

        public void Apply(BuffsModel buffs)
        {
            strengthBuff = buffs.Strength;
            agilityBuff = buffs.Agility;
            staminaBuff = buffs.Stamina;
            intelligenceBuff = buffs.Intelligence;
            senseBuff = buffs.Sense;
            psychicBuff = buffs.Psychic;
            treatmentBuff = buffs.Treatment;
            OnBuffsApplied?.Invoke();
        }

        public void UrlTokenDeserialize(Queue<object> data)
        {
            baseStrength = Convert.ToInt32(Convert.ToString(data.Dequeue()));
            baseAgility = Convert.ToInt32(Convert.ToString(data.Dequeue()));
            baseStamina = Convert.ToInt32(Convert.ToString(data.Dequeue()));
            baseIntelligence = Convert.ToInt32(Convert.ToString(data.Dequeue()));
            baseSense = Convert.ToInt32(Convert.ToString(data.Dequeue()));
            basePsychic = Convert.ToInt32(Convert.ToString(data.Dequeue()));
            baseTreatment = Convert.ToDouble(Convert.ToString(data.Dequeue()));
        }

        public void UrlTokenSerialize(Queue<object> data)
        {
            data.Enqueue(baseStrength);
            data.Enqueue(baseAgility);
            data.Enqueue(baseStamina);
            data.Enqueue(baseIntelligence);
            data.Enqueue(baseSense);
            data.Enqueue(basePsychic);
            data.Enqueue(baseTreatment);
        }
    }
}
