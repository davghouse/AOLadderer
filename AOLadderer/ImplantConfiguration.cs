using System.Collections.Generic;
using System.Linq;

namespace AOLadderer
{
    public class ImplantConfiguration
    {
        private Dictionary<ImplantSlot, Implant> _implantSlotImplants;

        public ImplantConfiguration(IEnumerable<Implant> implants = null)
            => _implantSlotImplants = ImplantSlot.ImplantSlots
            .ToDictionary(s => s, s => implants?.SingleOrDefault(i => i.ImplantSlot == s));

        public ImplantConfiguration(ImplantConfiguration implantConfiguration)
            => _implantSlotImplants = new Dictionary<ImplantSlot, Implant>(
                implantConfiguration._implantSlotImplants);

        public Implant GetImplant(ImplantSlot implantSlot)
            => _implantSlotImplants[implantSlot];

        public IEnumerable<Implant> GetEquippedImplants()
            => _implantSlotImplants.Values.Where(i => i != null);

        public void RemoveImplant(ImplantSlot implantSlot)
            => _implantSlotImplants[implantSlot] = null;

        public void SetImplant(Implant implant)
            => _implantSlotImplants[implant.ImplantSlot] = implant;

        public bool IsImplantSlotFull(ImplantSlot implantSlot)
            => _implantSlotImplants[implantSlot] != null;

        public bool IsImplantSlotEmpty(ImplantSlot implantSlot)
            => _implantSlotImplants[implantSlot] == null;

        public int GetTotalImplantQL(IEnumerable<ImplantSlot> implantSlots = null)
            => (implantSlots ?? ImplantSlot.ImplantSlots)
            .Select(GetImplant)
            .Sum(i => i?.QL ?? 0);

        public double GetAverageImplantQL(IEnumerable<ImplantSlot> implantSlots = null)
        {
            int equippedImplantCount = (implantSlots ?? ImplantSlot.ImplantSlots)
                .Where(IsImplantSlotFull)
                .Count();

            return equippedImplantCount == 0 ? 0 : GetTotalImplantQL(implantSlots) / (double)equippedImplantCount;
        }
    }
}
