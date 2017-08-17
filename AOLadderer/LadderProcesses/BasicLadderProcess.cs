using System;
using System.Collections.Generic;
using System.Linq;

namespace AOLadderer.LadderProcesses
{
    public sealed class BasicLadderProcess : LadderProcess
    {
        public BasicLadderProcess(Character character, IEnumerable<ImplantTemplate> finalImplantTemplates)
            : base(character, finalImplantTemplates)
        {
            if (_character.GetEquippedImplants().Any())
                throw new InvalidOperationException("To start the basic ladder process, all implants must be unequipped.");

            Run();
        }

        protected override void Run()
        {
            while (true)
            {
                Implant nextLadderImplant = null;

                foreach (var emptyLadderImplantSlot in ImplantSlot.LadderImplantSlots
                    .Where(_character.IsImplantSlotEmpty))
                {
                    foreach (var relevantLadderImplantGroup in emptyLadderImplantSlot.LadderImplantGroups
                        .Where(g => !g.IsSupersededByOtherLadderImplantGroups(_character)))
                    {
                        var trialLadderImplantTemplate = relevantLadderImplantGroup.GetBestImplantTemplate(_character);
                        var trialLadderImplant = _character
                            .EquipMaxImplant(trialLadderImplantTemplate, isSlotKnownToBeEmpty: true);
                        var trialDegenerateLadderProcess = new DegenerateLadderProcess(_character, _finalImplantTemplates);
                        int trialTotalFinalImplantQL = trialDegenerateLadderProcess.TotalFinalImplantQL;
                        var trialOrderedFinalImplants = trialDegenerateLadderProcess.OrderedFinalImplants;

                        if (trialTotalFinalImplantQL > TotalFinalImplantQL)
                        {
                            TotalFinalImplantQL = trialTotalFinalImplantQL;
                            _orderedFinalImplants = trialOrderedFinalImplants;
                            nextLadderImplant = trialLadderImplant;
                        }

                        _character.UnequipImplant(trialLadderImplant.ImplantSlot);
                    }
                }

                if (nextLadderImplant != null)
                {
                    _character.SetImplant(nextLadderImplant, isSlotKnownToBeEmpty: true);
                    _orderedLadderImplants.Add(nextLadderImplant);
                }
                else break;
            }

            _character.UnequipImplants();
        }

        private List<Implant> _orderedLadderImplants = new List<Implant>();
        public override IReadOnlyList<Implant> OrderedLadderImplants => _orderedLadderImplants;

        private IReadOnlyList<Implant> _orderedFinalImplants = new Implant[0];
        public override IReadOnlyList<Implant> OrderedFinalImplants => _orderedFinalImplants;
    }
}
