using AOLadderer.Helpers;
using AOLadderer.Stats;
using System.Collections.Generic;
using System.Linq;

namespace AOLadderer
{
    public abstract class LadderProcess
    {
        protected Character _character;
        protected ImplantTemplate[] _finalImplantTemplates;
        protected ImplantTemplate[] _finalLadderImplantTemplates;
        protected ImplantTemplate[] _finalNonLadderImplantTemplates;
        protected ImplantTemplateConfiguration _finalImplantTemplateConfiguration;

        private LadderProcess(IEnumerable<ImplantTemplate> implantTemplates)
        {
            _finalImplantTemplates = implantTemplates.ToArray();
            _finalLadderImplantTemplates = _finalImplantTemplates.Where(t => t.RaisesLadderStats).ToArray();
            _finalNonLadderImplantTemplates = _finalImplantTemplates.Where(t => !t.RaisesLadderStats).ToArray();
            _finalImplantTemplateConfiguration = new ImplantTemplateConfiguration(_finalImplantTemplates);
        }

        public LadderProcess(
            int agilityValue, int intelligenceValue, int psychicValue, int senseValue,
            int staminaValue, int strengthValue, double treatmentValue,
            IEnumerable<ImplantTemplate> implantTemplates)
            : this(implantTemplates)
        {
            _character = new Character(
                agilityValue, intelligenceValue, psychicValue, senseValue,
                staminaValue, strengthValue, treatmentValue);
            Run();
        }

        public LadderProcess(
            IReadOnlyDictionary<Ability, int> abilityValues, double treatmentValue,
            IEnumerable<ImplantTemplate> implantTemplates)
            : this(implantTemplates)
        {
            _character = new Character(abilityValues, treatmentValue);
            Run();
        }

        public LadderProcess(Character character, IEnumerable<ImplantTemplate> implantTemplates)
            : this(implantTemplates)
        {
            _character = character;
            _character.UnequipImplants();
            Run();
        }

        protected abstract void Run();

        public int TotalImplantQL { get; protected set; }
        public double AverageImplantQL => TotalImplantQL / (double)_finalImplantTemplates.Length;

        protected List<Implant> _orderedLadderImplants = new List<Implant>();
        public IReadOnlyList<Implant> OrderedLadderImplants => _orderedLadderImplants;

        protected List<Implant> _orderedFinalImplants = new List<Implant>();
        public IReadOnlyList<Implant> OrderedFinalImplants => _orderedFinalImplants;

        // Similar to https://stackoverflow.com/a/756083. Note we have to consume this one iteration at a time, as
        // we're using the same array throughout the entire process. It's like for ABCD, do A[recurse on BCD], then
        // do B[recurse on ACD], do C[recurse on ABD] and so on, where the swapping is what moves the left value
        // out of and back into the subarray being recursed on.
        public static IEnumerable<ImplantTemplate[]> GetAllPossibleEquipOrders(
            ImplantTemplate[] implantTemplates, int swapIndex = 0)
        {
            if (swapIndex == implantTemplates.Length - 1)
            {
                yield return implantTemplates;
            }
            else
            {
                for (int i = swapIndex; i < implantTemplates.Length; ++i)
                {
                    implantTemplates.Swap(swapIndex, i);

                    foreach (var equipOrder in GetAllPossibleEquipOrders(implantTemplates, swapIndex + 1))
                        yield return equipOrder;

                    implantTemplates.Swap(i, swapIndex);
                }
            }
        }
    }
}
