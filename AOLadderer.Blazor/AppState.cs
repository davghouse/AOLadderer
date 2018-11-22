using AOLadderer.Blazor.Models;
using AOLadderer.LadderProcesses;
using System.Diagnostics;
using System.Linq;

namespace AOLadderer.Blazor
{
    public static class AppState
    {
        public static ImplantsModel Implants { get; set; } = new ImplantsModel();
        public static StatsModel Stats { get; set; } = new StatsModel();
        public static BuffsModel Buffs { get; set; } = new BuffsModel();
        public static LadderModel Ladder { get; set; }

        public static void InvalidateLadder()
        {
            Debug.WriteLine("Invalidating ladder...");
            Ladder = null;
        }

        public static void InitializeLadder()
        {
            if (Ladder != null) return;

            var character = new Character(Stats.Agility, Stats.Intelligence, Stats.Psychic, Stats.Sense, Stats.Stamina, Stats.Strength, Stats.Treatment);
            var implantTemplates = Implants.Select(i => i.GetImplantTemplate()).Where(i => i != null).ToArray();

            Debug.WriteLine("Building ladder...");
            var ladderProcess = new BasicLadderProcess(character, implantTemplates);

            Debug.WriteLine("Ladder built.");
            Ladder = new LadderModel(ladderProcess);
        }
    }
}
