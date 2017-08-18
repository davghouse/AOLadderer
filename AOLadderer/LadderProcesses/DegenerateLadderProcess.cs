using System.Collections.Generic;
using System.Linq;

namespace AOLadderer.LadderProcesses
{
    public sealed class DegenerateLadderProcess : LadderProcess
    {
        private readonly IReadOnlyList<Implant> _initiallyEquippedImplants;
        private readonly ImplantTemplate[] _finalLadderImplantTemplatesInInitiallyEmptyImplantSlots;
        private readonly IReadOnlyList<ImplantTemplate> _finalNonLadderImplantTemplatesInInitiallyEmptyImplantSlots;
        private readonly ImplantTemplate[] _finalImplantTemplatesInInitiallyFullImplantSlots;

        public DegenerateLadderProcess(Character character, IEnumerable<ImplantTemplate> finalImplantTemplates)
            : base(character, finalImplantTemplates)
        {
            _initiallyEquippedImplants = _character.GetEquippedImplants().ToArray();
            _finalLadderImplantTemplatesInInitiallyEmptyImplantSlots = _finalImplantTemplates
                .Where(t => t.RaisesLadderStats && _character.IsImplantSlotEmpty(t.ImplantSlot))
                .ToArray();
            _finalNonLadderImplantTemplatesInInitiallyEmptyImplantSlots = _finalImplantTemplates
                .Where(t => !t.RaisesLadderStats && _character.IsImplantSlotEmpty(t.ImplantSlot))
                .ToArray();
            _finalImplantTemplatesInInitiallyFullImplantSlots = _finalImplantTemplates
                .Where(t => _character.IsImplantSlotFull(t.ImplantSlot))
                .ToArray();
            Run();
        }

        protected override void Run()
        {
            foreach (var initiallyEmptyEquipOrder in GetAllPossibleEquipOrders(_finalLadderImplantTemplatesInInitiallyEmptyImplantSlots))
            {
                // Before, we were unequipping all implants and re-equipping the initial ones at the end of this loop body.
                // Turns out just creating a new character based on the original character is better for performance.
                var workingCharacter = new Character(_character);

                foreach (var finalLadderImplantTemplate in initiallyEmptyEquipOrder)
                {
                    workingCharacter.EquipMaxImplant(finalLadderImplantTemplate, isSlotKnownToBeEmpty: true);
                }

                foreach (var finalNonLadderImplantTemplate in _finalNonLadderImplantTemplatesInInitiallyEmptyImplantSlots)
                {
                    workingCharacter.EquipMaxImplant(finalNonLadderImplantTemplate, isSlotKnownToBeEmpty: true);
                }

                // We are assuming initially full slots are ladder implants which someone (the basic ladder process) is trying
                // out to see how much they improve the result of the degenerate ladder process. That's why we don't even consider
                // unequipping them until now. And given that they're ladder implants, we try all possible (un)equip orders.
                foreach (var initiallyFullEquipOrder in GetAllPossibleEquipOrders(_finalImplantTemplatesInInitiallyFullImplantSlots))
                {
                    foreach (var finalImplantTemplate in initiallyFullEquipOrder)
                    {
                        workingCharacter.EquipMaxImplant(finalImplantTemplate, isSlotKnownToBeEmpty: false);
                    }

                    int trialTotalFinalImplantQL = workingCharacter.GetTotalImplantQL(_finalImplantSlots);
                    if (trialTotalFinalImplantQL > TotalFinalImplantQL)
                    {
                        TotalFinalImplantQL = trialTotalFinalImplantQL;
                        _orderedFinalImplants = initiallyEmptyEquipOrder
                            .Concat(_finalNonLadderImplantTemplatesInInitiallyEmptyImplantSlots)
                            .Concat(initiallyFullEquipOrder)
                            .Select(t => workingCharacter.GetImplant(t.ImplantSlot))
                            .ToArray();
                    }

                    workingCharacter.SetImplants(_initiallyEquippedImplants, areSlotsKnownToBeEmpty: false);
                }
            }
        }

        public override IReadOnlyList<Implant> OrderedLadderImplants => new Implant[0];

        private IReadOnlyList<Implant> _orderedFinalImplants = new Implant[0];
        public override IReadOnlyList<Implant> OrderedFinalImplants => _orderedFinalImplants;
    }
}
