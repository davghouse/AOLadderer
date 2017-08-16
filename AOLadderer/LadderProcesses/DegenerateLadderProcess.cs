using AOLadderer.Stats;
using System.Collections.Generic;
using System.Linq;

namespace AOLadderer.LadderProcesses
{
    public sealed class DegenerateLadderProcess : LadderProcess
    {
        public DegenerateLadderProcess(
            int agilityValue, int intelligenceValue, int psychicValue, int senseValue,
            int staminaValue, int strengthValue, double treatmentValue,
            IEnumerable<ImplantTemplate> implantTemplates)
            : base(agilityValue, intelligenceValue, psychicValue, senseValue,
                  staminaValue, strengthValue, treatmentValue, implantTemplates)
        { }

        public DegenerateLadderProcess(
            IReadOnlyDictionary<Ability, int> abilityValues, double treatmentValue,
            IEnumerable<ImplantTemplate> implantTemplates)
            : base(abilityValues, treatmentValue, implantTemplates)
        { }

        public DegenerateLadderProcess(Character character, IEnumerable<ImplantTemplate> implantTemplates)
            : base(character, implantTemplates)
        { }

        protected override void Run()
        {
            if (_finalLadderImplantTemplates.Length == 0)
            {
                foreach (var finalImplantTemplate in _finalImplantTemplates)
                {
                    _character.EquipMaxImplant(finalImplantTemplate, isSlotKnownToBeEmpty: true);
                }

                TotalImplantQL = _character.GetTotalImplantQL();
                _orderedFinalImplants = _finalImplantTemplates
                    .Select(t => _character.GetImplant(t.ImplantSlot))
                    .ToList();
                _character.UnequipImplants();
            }
            else
            {
                foreach (var equipOrder in GetAllPossibleEquipOrders(_finalLadderImplantTemplates))
                {
                    foreach (var finalLadderImplantTemplate in equipOrder)
                    {
                        _character.EquipMaxImplant(finalLadderImplantTemplate, isSlotKnownToBeEmpty: true);
                    }

                    foreach (var finalNonLadderImplantTemplate in _finalNonLadderImplantTemplates)
                    {
                        _character.EquipMaxImplant(finalNonLadderImplantTemplate, isSlotKnownToBeEmpty: true);
                    }

                    int equipOrderTotalImplantQL = _character.GetTotalImplantQL();
                    if (equipOrderTotalImplantQL > TotalImplantQL)
                    {
                        TotalImplantQL = equipOrderTotalImplantQL;
                        _orderedFinalImplants = equipOrder
                            .Concat(_finalNonLadderImplantTemplates)
                            .Select(t => _character.GetImplant(t.ImplantSlot))
                            .ToList();
                    }

                    _character.UnequipImplants();
                }
            }
        }
    }
}
