using AOLadderer.Blazor.Models;

namespace AOLadderer.Blazor
{
    public static class AppState
    {
        public static ImplantsModel Implants { get; set; } = new ImplantsModel();
        public static StatsModel Stats { get; set; } = new StatsModel();
        public static BuffsModel Buffs { get; set; } = new BuffsModel();
    }
}
