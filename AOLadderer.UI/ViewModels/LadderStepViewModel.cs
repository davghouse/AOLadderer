namespace AOLadderer.UI.ViewModels
{
    public class LadderStepViewModel : ViewModelBase
    {
        public LadderStepViewModel(Implant implant)
            => Implant = implant;

        public Implant Implant { get; }

        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set => Set(ref _isChecked, value);
        }

        public int ImplantQL => Implant.QL;
        public ImplantSlot ImplantSlot => Implant.ImplantSlot;
        public string ShinyStat => Implant.ShinyStat?.Name;
        public string BrightStat => Implant.BrightStat?.Name;
        public string FadedStat => Implant.FadedStat?.Name;
    }
}
