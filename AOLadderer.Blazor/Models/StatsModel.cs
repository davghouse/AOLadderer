using System;

namespace AOLadderer.Blazor.Models
{
    public class StatsModel
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
    }
}
