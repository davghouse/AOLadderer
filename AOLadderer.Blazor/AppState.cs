using AOLadderer.Blazor.Models;
using System.Diagnostics;

namespace AOLadderer.Blazor
{
    public class AppState
    {
        public BuildModel Build { get; set; } = new BuildModel();
        public bool UseAdvancedLadderProcess { get; set; }
        public LadderModel Ladder { get; set; }
        public ShoppingModel Shopping { get; set; }

        public void InvalidateLadder()
        {
            Debug.WriteLine("Invalidating ladder...");
            Ladder = null;
            Shopping = null;
        }

        public void InitializeLadder()
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
