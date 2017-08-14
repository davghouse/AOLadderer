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

        public Implant GetImplant(ImplantSlot implantSlot)
            => _implantSlotImplants[implantSlot];

        public void RemoveImplant(ImplantSlot implantSlot)
            => _implantSlotImplants[implantSlot] = null;

        public void SetImplant(Implant implant)
            => _implantSlotImplants[implant.ImplantSlot] = implant;

        public bool IsImplantSlotFull(ImplantSlot implantSlot)
            => _implantSlotImplants[implantSlot] != null;

        public bool IsImplantSlotEmpty(ImplantSlot implantSlot)
            => _implantSlotImplants[implantSlot] == null;
    }
}
