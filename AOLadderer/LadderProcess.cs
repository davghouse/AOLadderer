using AOLadderer.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace AOLadderer
{
    public abstract class LadderProcess
    {
        protected readonly Character _character;
        protected readonly IReadOnlyList<ImplantTemplate> _finalImplantTemplates;
        protected readonly IReadOnlyList<ImplantSlot> _finalImplantSlots;

        public LadderProcess(Character character, IEnumerable<ImplantTemplate> finalImplantTemplates)
        {
            _character = character;
            _finalImplantTemplates = finalImplantTemplates.ToArray();
            _finalImplantSlots = finalImplantTemplates.Select(t => t.ImplantSlot).ToArray();
        }

        protected abstract void Run();
        public int TotalFinalImplantQL { get; protected set; }
        public double AverageFinalImplantQL
            => _finalImplantTemplates.Count == 0 ? 0 : TotalFinalImplantQL / (double)_finalImplantTemplates.Count;
        public abstract IReadOnlyList<Implant> OrderedLadderImplants { get; }
        public abstract IReadOnlyList<Implant> OrderedFinalImplants { get; }

        // Convenient to return a single empty array if the input array is empty.
        public static IEnumerable<ImplantTemplate[]> GetAllPossibleEquipOrders(ImplantTemplate[] implantTemplates)
            => implantTemplates.Length == 0 ? new[] { implantTemplates } : GetAllPossibleEquipOrders(implantTemplates, 0);

        // Similar to https://stackoverflow.com/a/756083. Note we have to consume this one iteration at a time, as
        // we're using the same array throughout the entire process. It's like for ABCD, do A[recurse on BCD], then
        // do B[recurse on ACD], do C[recurse on ABD] and so on, where the swapping is what moves the left value
        // out of and back into the subarray being recursed on.
        public static IEnumerable<ImplantTemplate[]> GetAllPossibleEquipOrders(
            ImplantTemplate[] implantTemplates, int swapIndex)
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
