using AOLadderer.Stats;
using System.Collections.Generic;
using System.Linq;

namespace AOLadderer
{
    public class LadderStatValues
    {
        private Dictionary<Ability, int> _abilityValues;

        public LadderStatValues(int agilityValue, int intelligenceValue, int psychicValue,
            int senseValue, int staminaValue, int strengthValue, double treatmentValue)
        {
            _abilityValues = Ability.Abilities.ToDictionary(a => a, a => 0);
            _abilityValues[Ability.Agility] = agilityValue;
            _abilityValues[Ability.Intelligence] = intelligenceValue;
            _abilityValues[Ability.Psychic] = psychicValue;
            _abilityValues[Ability.Sense] = senseValue;
            _abilityValues[Ability.Stamina] = staminaValue;
            _abilityValues[Ability.Strength] = strengthValue;
            TreatmentValue = treatmentValue;
        }

        public LadderStatValues(IReadOnlyDictionary<Ability, int> abilityValues = null, double treatmentValue = 0)
        {
            _abilityValues = Ability.Abilities
                .ToDictionary(a => a, a => (abilityValues?.ContainsKey(a) ?? false) ? abilityValues[a] : 0);
            TreatmentValue = treatmentValue;
        }

        public int GetAbilityValue(Ability ability)
            => _abilityValues[ability];

        public void SetAbilityValue(Ability ability, int abilityValue)
            => _abilityValues[ability] = abilityValue;

        public double TreatmentValue { get; set; }

        public void IncreaseAbilityValue(Ability ability, int abilityIncrease)
        {
            _abilityValues[ability] += abilityIncrease;
            TreatmentValue += ability.GetTreatmentTrickle(abilityIncrease);
        }

        public void DecreaseAbilityValue(Ability ability, int abilityDecrease)
        {
            _abilityValues[ability] -= abilityDecrease;
            TreatmentValue -= ability.GetTreatmentTrickle(abilityDecrease);
        }

        public void AddImplantValues(Implant implant)
        {
            foreach (var abilityClusterTemplate in implant.AbilityClusterTemplates)
            {
                IncreaseAbilityValue(
                    abilityClusterTemplate.Ability,
                    abilityClusterTemplate.GetStatIncrease(implant.ImplantQL));
            }

            TreatmentValue += implant.TreatmentClusterTemplate?.GetStatIncrease(implant.ImplantQL) ?? 0;
        }

        public void SubtractImplantValues(Implant implant)
        {
            foreach (var abilityClusterTemplate in implant.AbilityClusterTemplates)
            {
                DecreaseAbilityValue(
                    abilityClusterTemplate.Ability,
                    abilityClusterTemplate.GetStatIncrease(implant.ImplantQL));
            }

            TreatmentValue -= implant.TreatmentClusterTemplate?.GetStatIncrease(implant.ImplantQL) ?? 0;
        }
    }
}
