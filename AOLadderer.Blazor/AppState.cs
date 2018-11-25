using AOLadderer.Blazor.Models;
using System.Diagnostics;

namespace AOLadderer.Blazor
{
    public class AppState
    {
        public BuildModel Build { get; set; } = new BuildModel();
        public bool UseAdvancedLadderProcess { get; set; }
        public bool IsInitialized { get; set; }
        public LadderModel Ladder { get; set; }
        public ShoppingModel Shopping { get; set; }
        public ExportToAunoModel ExportToAuno { get; set; }

        public void InvalidateLadder()
        {
            Debug.WriteLine("Invalidating ladder...");
            Ladder = null;
            Shopping = null;
            ExportToAuno = null;
            IsInitialized = false;
        }

        public void InitializeLadder()
        {
            if (IsInitialized) return;

            Debug.WriteLine("Building ladder...");
            LadderProcess ladder = Build.CreateLadderProcess(UseAdvancedLadderProcess);

            Debug.WriteLine("Ladder built.");
            Ladder = new LadderModel(ladder);
            Shopping = new ShoppingModel(ladder);
            ExportToAuno = new ExportToAunoModel(ladder);
        }
    }
}
