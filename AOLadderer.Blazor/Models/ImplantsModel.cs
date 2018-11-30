using AOLadderer.ClusterTemplates;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AOLadderer.Blazor.Models
{
    public class ImplantModel
    {
        private readonly Dictionary<string, ShinyClusterTemplate> shinyClusterHash;
        private readonly Dictionary<string, BrightClusterTemplate> brightClusterHash;
        private readonly Dictionary<string, FadedClusterTemplate> fadedClusterHash;

        public ImplantModel(ImplantSlot slot)
        {
            this.Slot = slot;
            shinyClusterHash = slot.ShinyClusterTemplates.ToDictionary(t => t.Stat.Name);
            brightClusterHash = slot.BrightClusterTemplates.ToDictionary(t => t.Stat.Name);
            fadedClusterHash = slot.FadedClusterTemplates.ToDictionary(t => t.Stat.Name);
        }

        public ImplantSlot Slot { get; }
        public string ShinyClusterSelection { get; set; } = string.Empty; // Empty string is more compact than null when serialized.
        public string BrightClusterSelection { get; set; } = string.Empty; // Empty string is more compact than null when serialized.
        public string FadedClusterSelection { get; set; } = string.Empty; // Empty string is more compact than null when serialized.
        public bool IsUnavailable { get; set; }

        public IReadOnlyCollection<string> ShinyClusterOptions => shinyClusterHash.Keys;
        public IReadOnlyCollection<string> BrightClusterOptions => brightClusterHash.Keys;
        public IReadOnlyCollection<string> FadedClusterOptions => fadedClusterHash.Keys;
        
        public ImplantTemplate CreateImplantTemplate()
        {
            shinyClusterHash.TryGetValue(ShinyClusterSelection ?? string.Empty, out ShinyClusterTemplate shinyCluster);
            brightClusterHash.TryGetValue(BrightClusterSelection ?? string.Empty, out BrightClusterTemplate brightCluster);
            fadedClusterHash.TryGetValue(FadedClusterSelection ?? string.Empty, out FadedClusterTemplate fadedCluster);
            if (shinyCluster == null && brightCluster == null && fadedCluster == null) return null;

            return ImplantTemplate.GetImplantTemplate(Slot, shinyCluster?.Stat, brightCluster?.Stat, fadedCluster?.Stat);
        }

        public string GetLabel()
        {
            ImplantTemplate template = CreateImplantTemplate();
            if (template == null) return Slot.ShortName;

            return $"{template.RequiredAbility.ShortName} {Slot.ShortName}";
        }
    }

    public class ImplantsModel : IReadOnlyCollection<ImplantModel>, IUrlTokenSerializable
    {
        private readonly IReadOnlyCollection<ImplantModel> implants
            = ImplantSlot.ImplantSlots
            .Select(s => new ImplantModel(s))
            .ToArray();

        public int Count => implants.Count;
        public IEnumerator<ImplantModel> GetEnumerator() => implants.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => implants.GetEnumerator();

        public void UrlTokenDeserialize(Queue<object> data)
        {
            foreach (ImplantModel implant in implants)
            {
                implant.ShinyClusterSelection = Convert.ToString(data.Dequeue());
                implant.BrightClusterSelection = Convert.ToString(data.Dequeue());
                implant.FadedClusterSelection = Convert.ToString(data.Dequeue());
                implant.IsUnavailable = Convert.ToInt32(data.Dequeue()) == 1;
            }
        }

        public void UrlTokenSerialize(Queue<object> data)
        {
            foreach (ImplantModel implant in implants)
            {
                data.Enqueue(implant.ShinyClusterSelection);
                data.Enqueue(implant.BrightClusterSelection);
                data.Enqueue(implant.FadedClusterSelection);
                data.Enqueue(implant.IsUnavailable ? 1 : 0);
            }
        }
    }
}
