using AOLadderer.Blazor.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AOLadderer.Blazor
{
    public class AppState
    {
        public BuildModel Build { get; set; } = new BuildModel();
        public bool HasBuiltLadderProcess { get; set; }
        public LadderModel Ladder { get; set; }
        public ShoppingModel Shopping { get; set; }
        public ExportToAunoModel ExportToAuno { get; set; }

        public void InvalidateLadderProcess()
        {
            Debug.WriteLine("Invalidating ladder process...");
            Ladder = null;
            Shopping = null;
            ExportToAuno = null;
            HasBuiltLadderProcess = false;
        }

        public async Task InitializeLadderProcess()
        {
            if (HasBuiltLadderProcess) return;

            Debug.WriteLine("Building ladder process...");
            // Browsers are single-threaded so this will actually still block the UI.
            // See: https://github.com/aspnet/Blazor/issues/560
            LadderProcess ladder = await Task.Run(() => Build.CreateLadderProcess());

            Debug.WriteLine("Ladder process built.");
            Ladder = new LadderModel(ladder);
            Shopping = new ShoppingModel(ladder);
            ExportToAuno = new ExportToAunoModel(ladder);
            HasBuiltLadderProcess = true;
        }
    }
}
