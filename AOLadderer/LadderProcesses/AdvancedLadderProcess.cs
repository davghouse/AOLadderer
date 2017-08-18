using System;
using System.Collections.Generic;
using System.Linq;

namespace AOLadderer.LadderProcesses
{
    public sealed class AdvancedLadderProcess : LadderProcess
    {
        // Given a character with no initially equipped implants and their final implant templates, use the basic
        // ladder process to get an idea for which ladder implants are helpful. Then use the basic ladder process
        // again to ladder up before equipping those ladder implants, and then finally equip the final implants.
        // It's actually not guaranteed to be better than the basic ladder process, because it tries to maximize
        // the total QL of the ladder implants being used to ladder the final implants, which may change the order
        // of those ladder implants relative to the initial basic ladder process, and therefore add more to ladder
        // stats unimportant to that process and less to ladder stats important to it. I'm betting that usually
        // it'll be worth it to trust the order that maximizes the QL of the ladder implants, rather than forcing
        // them into the order found in the initial basic ladder process.
        public AdvancedLadderProcess(Character character, IEnumerable<ImplantTemplate> finalImplantTemplates)
            : base(character, finalImplantTemplates)
        {
            if (_character.GetEquippedImplants().Any())
                throw new InvalidOperationException("To start the advanced ladder process, all implants must be unequipped.");

            Run();
        }

        protected override void Run()
        {
            var basicLadderProcess = new BasicLadderProcess(_character, _finalImplantTemplates);
            var ladderImplantTemplatesToLadder = basicLadderProcess.OrderedLadderImplants.Select(i => i.ImplantTemplate);
            var ladderImplantBasicLadderProcess = new BasicLadderProcess(_character, ladderImplantTemplatesToLadder);

            foreach (var orderedLadderLadderImplant in ladderImplantBasicLadderProcess.OrderedLadderImplants)
            {
                _character.SetImplant(orderedLadderLadderImplant, isSlotKnownToBeEmpty: true);
                _orderedLadderImplants.Add(orderedLadderLadderImplant);
            }

            foreach (var orderedLadderImplant in ladderImplantBasicLadderProcess.OrderedFinalImplants)
            {
                _character.SetImplant(orderedLadderImplant, isSlotKnownToBeEmpty: false);
                _orderedLadderImplants.Add(orderedLadderImplant);
            }

            var degenerateLadderProcess = new DegenerateLadderProcess(_character, _finalImplantTemplates);
            TotalFinalImplantQL = degenerateLadderProcess.TotalFinalImplantQL;
            _orderedFinalImplants = degenerateLadderProcess.OrderedFinalImplants;

            _character.UnequipImplants();
        }

        private List<Implant> _orderedLadderImplants = new List<Implant>();
        public override IReadOnlyList<Implant> OrderedLadderImplants => _orderedLadderImplants;

        private IReadOnlyList<Implant> _orderedFinalImplants;
        public override IReadOnlyList<Implant> OrderedFinalImplants => _orderedFinalImplants;
    }
}
