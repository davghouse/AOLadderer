using System.Collections.Generic;
using System.Linq;

namespace AOLadderer
{
    public class ImplantTemplateConfiguration
    {
        private Dictionary<ImplantSlot, ImplantTemplate> _implantSlotImplantTemplates;

        public ImplantTemplateConfiguration(IEnumerable<ImplantTemplate> implantTemplates = null)
            => _implantSlotImplantTemplates = ImplantSlot.ImplantSlots
            .ToDictionary(s => s, s => implantTemplates?.SingleOrDefault(t => t.ImplantSlot == s));

        public ImplantTemplateConfiguration(ImplantTemplateConfiguration implantTemplateConfiguration)
            => _implantSlotImplantTemplates = new Dictionary<ImplantSlot, ImplantTemplate>(
                implantTemplateConfiguration._implantSlotImplantTemplates);

        public ImplantTemplate GetImplantTemplate(ImplantSlot implantSlot)
            => _implantSlotImplantTemplates[implantSlot];

        public void RemoveImplantTemplate(ImplantSlot implantSlot)
            => _implantSlotImplantTemplates[implantSlot] = null;

        public void SetImplantTemplate(ImplantTemplate implantTemplate)
            => _implantSlotImplantTemplates[implantTemplate.ImplantSlot] = implantTemplate;

        public bool IsImplantSlotFull(ImplantSlot implantSlot)
            => _implantSlotImplantTemplates[implantSlot] != null;

        public bool IsImplantSlotEmpty(ImplantSlot implantSlot)
            => _implantSlotImplantTemplates[implantSlot] == null;
    }
}
