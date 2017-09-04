using System;
using System.Collections.Generic;
using System.Linq;

namespace AOLadderer.LadderProcesses
{
    // Given a character with no initially equipped implants (in available slots--some slots may be unavailabe to us)
    // and their final implant templates, use the basic ladder process to get an idea for which ladder implants
    // are helpful. Then use the basic ladder process again to ladder up before equipping those ladder implants,
    // and then finally equip the final implants. That's actually not guaranteed to be better than the basic ladder
    // process, because it tries to maximize the total QL of the ladder implants being used to ladder the final
    // implants, which may change the order of those ladder implants relative to the initial basic ladder process,
    // and therefore add more to ladder stats unimportant to that process and less to ladder stats important to it.
    // I'm betting that usually it'll be worth it to trust the order that maximizes the QL of the ladder implants,
    // rather than forcing them into the order found in the initial basic ladder process. However, if we find the
    // process isn't as good as the simpler basic ladder process, just use the basic ladder process.
    public sealed class AdvancedLadderProcess : LadderProcess
    {
        private readonly IReadOnlyList<ImplantSlot> _unavailableImplantSlots;
        private readonly IReadOnlyList<ImplantSlot> _availableImplantSlots;
        private readonly IReadOnlyList<ImplantSlot> _availableLadderImplantSlots;

        public AdvancedLadderProcess(Character character, IEnumerable<ImplantTemplate> finalImplantTemplates,
            IEnumerable<ImplantSlot> unavailableImplantSlots = null)
            : base(character, finalImplantTemplates)
        {
            _unavailableImplantSlots = unavailableImplantSlots?.ToArray() ?? new ImplantSlot[0];
            _availableImplantSlots = ImplantSlot.ImplantSlots.Except(_unavailableImplantSlots).ToArray();
            _availableLadderImplantSlots = ImplantSlot.LadderImplantSlots.Except(_unavailableImplantSlots).ToArray();

            if (_finalImplantSlots.Intersect(_unavailableImplantSlots).Any())
                throw new InvalidOperationException("It doesn't make sense to have slots for final implants be unavailable.");

            if (_availableImplantSlots.Any(_character.IsImplantSlotFull))
                throw new InvalidOperationException("All available implant slots must start as empty for the advanced ladder process."
                    + " Unequip implants first, or mark the slots as unavailable.");

            // Now we know all final implants are in available slots, and all available slots are empty.

            Run();
        }

        protected override void Run()
        {
            var workingCharacter = new Character(_character);

            var basicLadderProcess = new BasicLadderProcess(workingCharacter, _finalImplantTemplates, _unavailableImplantSlots);
            var ladderImplantTemplatesToLadder = basicLadderProcess.OrderedLadderImplants.Select(i => i.ImplantTemplate);
            var ladderImplantsBasicLadderProcess = new BasicLadderProcess(workingCharacter, ladderImplantTemplatesToLadder, _unavailableImplantSlots);

            foreach (var orderedLadderLadderImplant in ladderImplantsBasicLadderProcess.OrderedLadderImplants)
            {
                workingCharacter.SetImplant(orderedLadderLadderImplant, isSlotKnownToBeEmpty: true);
                _orderedLadderImplants.Add(orderedLadderLadderImplant);
            }

            foreach (var orderedLadderImplant in ladderImplantsBasicLadderProcess.OrderedFinalImplants)
            {
                workingCharacter.SetImplant(orderedLadderImplant, isSlotKnownToBeEmpty: false);
                _orderedLadderImplants.Add(orderedLadderImplant);
            }

            var trialDegenerateLadderProcess = new DegenerateLadderProcess(workingCharacter, _finalImplantTemplates);

            if (trialDegenerateLadderProcess.TotalFinalImplantQL > basicLadderProcess.TotalFinalImplantQL)
            {
                TotalFinalImplantQL = trialDegenerateLadderProcess.TotalFinalImplantQL;
                _orderedFinalImplants = trialDegenerateLadderProcess.OrderedFinalImplants;
            }
            else
            {
                TotalFinalImplantQL = basicLadderProcess.TotalFinalImplantQL;
                _orderedLadderImplants = basicLadderProcess.OrderedLadderImplants.ToList();
                _orderedFinalImplants = basicLadderProcess.OrderedFinalImplants;
            }
        }

        private List<Implant> _orderedLadderImplants = new List<Implant>();
        public override IReadOnlyList<Implant> OrderedLadderImplants => _orderedLadderImplants;

        private IReadOnlyList<Implant> _orderedFinalImplants;
        public override IReadOnlyList<Implant> OrderedFinalImplants => _orderedFinalImplants;
    }
}
