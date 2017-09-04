using System;
using System.Collections.Generic;
using System.Linq;

namespace AOLadderer.LadderProcesses
{
    // Given a character with no initially equipped implants (in available slots--some slots may be unavailabe to us)
    // and their final implant templates, greedily select ladder implants that produce the biggest increase to the
    // total final implant QL, if all final implants were to be equipped (in the optimal order) after the ladder
    // implant. This is done by carefully considering only the ladder implants which might actually help, and then
    // using the degenerate ladder process to assess how effective they are.
    public sealed class BasicLadderProcess : LadderProcess
    {
        private readonly IReadOnlyList<ImplantSlot> _unavailableImplantSlots;
        private readonly IReadOnlyList<ImplantSlot> _availableImplantSlots;
        private readonly IReadOnlyList<ImplantSlot> _availableLadderImplantSlots;

        public BasicLadderProcess(Character character, IEnumerable<ImplantTemplate> finalImplantTemplates,
            IEnumerable<ImplantSlot> unavailableImplantSlots = null)
            : base(character, finalImplantTemplates)
        {
            _unavailableImplantSlots = unavailableImplantSlots?.ToArray() ?? new ImplantSlot[0];
            _availableImplantSlots = ImplantSlot.ImplantSlots.Except(_unavailableImplantSlots).ToArray();
            _availableLadderImplantSlots = ImplantSlot.LadderImplantSlots.Except(_unavailableImplantSlots).ToArray();

            if (_finalImplantSlots.Intersect(_unavailableImplantSlots).Any())
                throw new InvalidOperationException("It doesn't make sense to have slots for final implants be unavailable.");

            if (_availableImplantSlots.Any(_character.IsImplantSlotFull))
                throw new InvalidOperationException("All available implant slots must start as empty for the basic ladder process."
                    + " Unequip implants first, or mark the slots as unavailable.");

            // Now we know all final implants are in available slots, and all available slots are empty.

            Run();
        }

        protected override void Run()
        {
            var workingCharacter = new Character(_character);

            while (true)
            {
                Implant nextLadderImplant = null;

                foreach (var emptyLadderImplantSlot in _availableLadderImplantSlots
                    .Where(workingCharacter.IsImplantSlotEmpty))
                {
                    foreach (var relevantLadderImplantGroup in emptyLadderImplantSlot.LadderImplantGroups
                        .Where(g => !g.IsSupersededByOtherLadderImplantGroups(workingCharacter)))
                    {
                        var trialLadderImplantTemplate = relevantLadderImplantGroup.GetBestImplantTemplate(workingCharacter);
                        var trialLadderImplant = workingCharacter
                            .EquipMaxImplant(trialLadderImplantTemplate, isSlotKnownToBeEmpty: true);
                        var trialDegenerateLadderProcess = new DegenerateLadderProcess(workingCharacter, _finalImplantTemplates);
                        int trialTotalFinalImplantQL = trialDegenerateLadderProcess.TotalFinalImplantQL;
                        var trialOrderedFinalImplants = trialDegenerateLadderProcess.OrderedFinalImplants;

                        if (trialTotalFinalImplantQL > TotalFinalImplantQL
                            // The old application used this heuristic: if we've found an implant useful for laddering, and
                            // we've found another 'just as good' that adds more treatment, prefer the one adding more treatment.
                            || (nextLadderImplant != null
                                && trialTotalFinalImplantQL == TotalFinalImplantQL
                                && trialLadderImplant.TotalTreatmentIncrease > nextLadderImplant.TotalTreatmentIncrease))
                        {
                            TotalFinalImplantQL = trialTotalFinalImplantQL;
                            _orderedFinalImplants = trialOrderedFinalImplants;
                            nextLadderImplant = trialLadderImplant;
                        }

                        workingCharacter.UnequipImplant(trialLadderImplant.ImplantSlot);
                    }
                }

                if (nextLadderImplant != null)
                {
                    workingCharacter.SetImplant(nextLadderImplant, isSlotKnownToBeEmpty: true);
                    _orderedLadderImplants.Add(nextLadderImplant);
                }
                else break;
            }

            // Avoid unnecessary ladder implants if the degenerate ladder process is just as good. It's convenient
            // for this check to come after the above, because the above implicitly guarantees we'll try at least 2
            // ladder implants deep (as long as 2+ slots are available) by virtue of TotalFinalImplantQL being
            // initialized to 0. This is important to handle the case where treatment and ability reqs are in perfect
            // balance w / each other s.t. only 1 ladder implant doesn't help at all. Doing this is only relevant
            // if we found < 2 ladder implants. If we found >= 2, it's because the 2nd ladder implant actually does help.
            if (_orderedLadderImplants.Count < 2)
            {
                workingCharacter = new Character(_character);

                var baseDegenerateLadderProcess = new DegenerateLadderProcess(workingCharacter, _finalImplantTemplates);
                int baseTotalFinalImplantQL = baseDegenerateLadderProcess.TotalFinalImplantQL;
                var baseOrderedFinalImplants = baseDegenerateLadderProcess.OrderedFinalImplants;

                if (baseTotalFinalImplantQL >= TotalFinalImplantQL)
                {
                    TotalFinalImplantQL = baseTotalFinalImplantQL;
                    _orderedLadderImplants.Clear();
                    _orderedFinalImplants = baseOrderedFinalImplants;
                }
            }
        }

        private List<Implant> _orderedLadderImplants = new List<Implant>();
        public override IReadOnlyList<Implant> OrderedLadderImplants => _orderedLadderImplants;

        private IReadOnlyList<Implant> _orderedFinalImplants = new Implant[0];
        public override IReadOnlyList<Implant> OrderedFinalImplants => _orderedFinalImplants;
    }
}
