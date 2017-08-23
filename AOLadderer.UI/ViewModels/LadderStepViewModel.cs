namespace AOLadderer.UI.ViewModels
{
    public class LadderStepViewModel : ViewModelBase
    {
        public LadderStepViewModel(Implant implant, bool isFinalImplant = false)
        {
            Implant = implant;
            IsFinalImplant = isFinalImplant;
        }

        public Implant Implant { get; }
        public bool IsFinalImplant { get; }

        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set => Set(ref _isChecked, value);
        }

        public int ImplantQL => Implant.QL;
        public ImplantSlot ImplantSlot => Implant.ImplantSlot;
        public ImplantTemplate ImplantTemplate => Implant.ImplantTemplate;
        public string ShinyStat => Implant.ShinyStat?.Name;
        public string BrightStat => Implant.BrightStat?.Name;
        public string FadedStat => Implant.FadedStat?.Name;
    }
}
