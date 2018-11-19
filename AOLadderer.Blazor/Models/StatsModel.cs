using System;

namespace AOLadderer.Blazor.Models
{
    public class StatsModel
    {
        public int StrengthBuff { get; set; }
        public int AgilityBuff { get; set; }
        public int StaminaBuff { get; set; }
        public int IntelligenceBuff { get; set; }
        public int SenseBuff { get; set; }
        public int PsychicBuff { get; set; }
        public double TreatmentBuff { get; set; }

        public int BaseStrength { get; set; }
        public int BaseAgility { get; set; }
        public int BaseStamina { get; set; }
        public int BaseIntelligence { get; set; }
        public int BaseSense { get; set; }
        public int BasePsychic { get; set; }
        public double BaseTreatment { get; set; }

        public int EffectiveStrength
        {
            get => BaseStrength + StrengthBuff;
            set => BaseStrength = value - StrengthBuff;
        }

        public int EffectiveAgility
        {
            get => BaseAgility + AgilityBuff;
            set => BaseAgility = value - AgilityBuff;
        }

        public int EffectiveStamina
        {
            get => BaseStamina + StaminaBuff;
            set => BaseStamina = value - StaminaBuff;
        }

        public int EffectiveIntelligence
        {
            get => BaseIntelligence + IntelligenceBuff;
            set => BaseIntelligence = value - IntelligenceBuff;
        }

        public int EffectiveSense
        {
            get => BaseSense + SenseBuff;
            set => BaseSense = value - SenseBuff;
        }

        public int EffectivePsychic
        {
            get => BasePsychic + PsychicBuff;
            set => BasePsychic = value - PsychicBuff;
        }

        public double EffectiveTreatment
        {
            get => BaseTreatment + TreatmentBuff;
            set => BaseTreatment = value - TreatmentBuff;
        }

        public event Action OnBuffsApplied;

        public void Apply(BuffsModel buffs)
        {
            StrengthBuff = buffs.Strength;
            AgilityBuff = buffs.Agility;
            StaminaBuff = buffs.Stamina;
            IntelligenceBuff = buffs.Intelligence;
            SenseBuff = buffs.Sense;
            PsychicBuff = buffs.Psychic;
            TreatmentBuff = buffs.Treatment;
            OnBuffsApplied?.Invoke();
        }
    }
}
