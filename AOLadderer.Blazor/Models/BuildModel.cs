using AOLadderer.LadderProcesses;
using System.Linq;

namespace AOLadderer.Blazor.Models
{
    public class BuildModel
    {
        public ImplantsModel Implants { get; } = new ImplantsModel();
        public StatsModel Stats { get; } = new StatsModel();
        public BuffsModel Buffs { get; } = new BuffsModel();

        public LadderProcess CreateLadderProcess(bool useAdvancedLadderProcess = false)
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

            if (useAdvancedLadderProcess)
                return new AdvancedLadderProcess(character, implantTemplates, unavailableImplantSlots);

            return new BasicLadderProcess(character, implantTemplates, unavailableImplantSlots);
        }
    }
}
