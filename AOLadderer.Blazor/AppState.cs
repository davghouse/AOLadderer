using AOLadderer.Blazor.Models;
using System.Diagnostics;

namespace AOLadderer.Blazor
{
    public static class AppState
    {
        public static BuildModel Build { get; set; } = new BuildModel();
        public static bool UseAdvancedLadderProcess { get; set; }
        public static LadderModel Ladder { get; set; }
        public static ShoppingModel Shopping { get; set; }

        public static void InvalidateLadder()
        {
            Debug.WriteLine("Invalidating ladder...");
            Ladder = null;
            Shopping = null;
        }

        public static void InitializeLadder()
        {
            if (Ladder != null && Shopping != null) return;

            Debug.WriteLine("Building ladder...");
            LadderProcess ladder = Build.CreateLadderProcess(UseAdvancedLadderProcess);

            Debug.WriteLine("Ladder built.");
            Ladder = new LadderModel(ladder);
            Shopping = new ShoppingModel(ladder);
        }
    }
}
