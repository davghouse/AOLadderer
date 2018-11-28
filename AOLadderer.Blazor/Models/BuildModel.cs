using AOLadderer.LadderProcesses;
using System.Collections.Generic;
using System.Linq;

namespace AOLadderer.Blazor.Models
{
    public class BuildModel : IUrlTokenSerializable
    {
        public ImplantsModel Implants { get; } = new ImplantsModel();
        public StatsModel Stats { get; } = new StatsModel();
        public BuffsModel Buffs { get; } = new BuffsModel();

        public LadderProcess CreateLadderProcess()
        {
            var character = new Character(
                Stats.Agility, Stats.Intelligence, Stats.Psychic,
                Stats.Sense, Stats.Stamina, Stats.Strength, Stats.Treatment);
            var implantTemplates = Implants
                .Where(i => !i.IsUnavailable)
                .Select(i => i.CreateImplantTemplate())
                .Where(i => i != null)
                .ToArray();
            var unavailableImplantSlots = Implants
                .Where(i => i.IsUnavailable)
                .Select(i => i.Slot)
                .ToArray();

            return new BasicLadderProcess(character, implantTemplates, unavailableImplantSlots);
        }

        public void UrlTokenDeserialize(Queue<object> data)
        {
            Implants.UrlTokenDeserialize(data);
            Stats.UrlTokenDeserialize(data);
            Buffs.UrlTokenDeserialize(data);

            Stats.Apply(Buffs);
        }

        public void UrlTokenSerialize(Queue<object> data)
        {
            Implants.UrlTokenSerialize(data);
            Stats.UrlTokenSerialize(data);
            Buffs.UrlTokenSerialize(data);
        }
    }
}
