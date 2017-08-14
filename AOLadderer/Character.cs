using AOLadderer.Stats;

namespace AOLadderer
{
    public class Character
    {
        private LadderStatValues _ladderStatValues;
        private ImplantConfiguration _implantConfiguration;

        public Character(LadderStatValues ladderStatValues = null, ImplantConfiguration implantConfiguration = null)
        {
            _ladderStatValues = ladderStatValues ?? new LadderStatValues();
            _implantConfiguration = implantConfiguration ?? new ImplantConfiguration();
        }

        public int GetAbilityValue(Ability ability)
            => _ladderStatValues.GetAbilityValue(ability);

        public void SetAbilityValue(Ability ability, int abilityValue)
            => _ladderStatValues.SetAbilityValue(ability, abilityValue);

        public void IncreaseAbilityValue(Ability ability, int abilityIncrease)
            => _ladderStatValues.IncreaseAbilityValue(ability, abilityIncrease);

        public void DecreaseAbilityValue(Ability ability, int abilityDecrease)
            => _ladderStatValues.DecreaseAbilityValue(ability, abilityDecrease);

        public double TreatmentValue
        {
            get => _ladderStatValues.TreatmentValue;
            set => _ladderStatValues.TreatmentValue = value;
        }

        public Implant GetImplant(ImplantSlot implantSlot)
            => _implantConfiguration.GetImplant(implantSlot);

        public Implant UnequipImplant(ImplantSlot implantSlot)
        {
            var implant = GetImplant(implantSlot);

            if (implant != null)
            {
                _ladderStatValues.SubtractImplantValues(implant);
                _implantConfiguration.RemoveImplant(implantSlot);
            }

            return implant;
        }

        private void SetImplantInSlotKnownToBeEmpty(Implant implant)
        {
            _implantConfiguration.SetImplant(implant);
            _ladderStatValues.AddImplantValues(implant);
        }

        public void SetImplant(Implant implant)
        {
            UnequipImplant(implant.ImplantSlot);
            SetImplantInSlotKnownToBeEmpty(implant);
        }

        public bool CanEquipImplant(Implant implant)
        {
            var currentImplant = UnequipImplant(implant.ImplantSlot);

            bool canEquipImplant = TreatmentValue >= implant.RequiredTreatmentValue
                && GetAbilityValue(implant.RequiredAbility) >= implant.RequiredAbilityValue;

            if (currentImplant != null)
            {
                SetImplantInSlotKnownToBeEmpty(currentImplant);
            }

            return canEquipImplant;
        }

        public bool TryEquipImplant(Implant implant)
        {
            var currentImplant = UnequipImplant(implant.ImplantSlot);

            bool canEquipImplant = TreatmentValue >= implant.RequiredTreatmentValue
                && GetAbilityValue(implant.RequiredAbility) >= implant.RequiredAbilityValue;

            if (canEquipImplant)
            {
                SetImplantInSlotKnownToBeEmpty(implant);
            }
            else if (currentImplant != null)
            {
                SetImplantInSlotKnownToBeEmpty(currentImplant);
            }

            return canEquipImplant;
        }

        public Implant GetMaxImplant(ImplantTemplate implantTemplate)
        {
            var currentImplant = UnequipImplant(implantTemplate.ImplantSlot);

            var maxImplant = new Implant(
                implantTemplate, Implant.GetMaxImplantQL(GetAbilityValue(implantTemplate.RequiredAbility), TreatmentValue));

            if (currentImplant != null)
            {
                SetImplantInSlotKnownToBeEmpty(currentImplant);
            }

            return maxImplant;
        }

        public Implant EquipMaxImplant(ImplantTemplate implantTemplate)
        {
            UnequipImplant(implantTemplate.ImplantSlot);

            var maxImplant = new Implant(
                implantTemplate, Implant.GetMaxImplantQL(GetAbilityValue(implantTemplate.RequiredAbility), TreatmentValue));

            SetImplantInSlotKnownToBeEmpty(maxImplant);

            return maxImplant;
        }

        public bool IsImplantSlotFull(ImplantSlot implantSlot)
            => _implantConfiguration.IsImplantSlotFull(implantSlot);

        public bool IsImplantSlotEmpty(ImplantSlot implantSlot)
            => _implantConfiguration.IsImplantSlotEmpty(implantSlot);
    }
}
