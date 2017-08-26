using AOLadderer.ClusterTemplates;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace AOLadderer.UI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            ExportBasicLadderToAuno = new RelayCommand(ExecuteExportBasicLadderToAuno);
            ExportAdvancedLadderToAuno = new RelayCommand(ExecuteExportAdvancedLadderToAuno);
            BuildViewModel.PropertyChanged += BuildViewModel_PropertyChanged;
        }

        public BuildViewModel BuildViewModel { get; } = new BuildViewModel();
        public LadderViewModel BasicLadderViewModel { get; } = new LadderViewModel();
        public ShoppingViewModel BasicShoppingViewModel { get; } = new ShoppingViewModel();
        public LadderViewModel AdvancedLadderViewModel { get; } = new LadderViewModel();
        public ShoppingViewModel AdvancedShoppingViewModel { get; } = new ShoppingViewModel();

        private int _selectedTabIndex = 0;
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set => Set(ref _selectedTabIndex, value);
        }

        // Well, this is kind of disgusting. I'll need to look into serialization options to replace this
        // manual effort. But I don't want serialization requirements to affect the AOLadderer project.
        // Regardless, this is inflexible. But AO is in maintenance mode and this project will be too.
        public string GetSave()
        {
            var saveBuilder = new StringBuilder();

            foreach (var implantConfigurationViewModel in BuildViewModel.ImplantConfigurationViewModels)
            {
                saveBuilder.AppendLine(implantConfigurationViewModel.GetImplantTemplate()?.AOID.ToString());
            }

            saveBuilder.AppendLine(BuildViewModel.UseComposites.ToString());
            saveBuilder.AppendLine(BuildViewModel.UseCompositesPay2Win.ToString());
            saveBuilder.AppendLine(BuildViewModel.UseSurgeryClinicEffect.ToString());
            saveBuilder.AppendLine(BuildViewModel.UseSuperiorFirstAid.ToString());
            saveBuilder.AppendLine(BuildViewModel.UseEnhancedSenses.ToString());
            saveBuilder.AppendLine(BuildViewModel.UseFelineGrace.ToString());
            saveBuilder.AppendLine(BuildViewModel.UseEssenceOfBehemoth.ToString());
            saveBuilder.AppendLine(BuildViewModel.UseGauntlet.ToString());
            saveBuilder.AppendLine(BuildViewModel.UseExtruderBar.ToString());
            saveBuilder.AppendLine(BuildViewModel.UseExplosifs.ToString());
            saveBuilder.AppendLine(BuildViewModel.UseIronCircle.ToString());
            saveBuilder.AppendLine(BuildViewModel.UseProdigiousStrength.ToString());
            saveBuilder.AppendLine(BuildViewModel.UseIronCircleOrProdigiousStrength.ToString());
            saveBuilder.AppendLine(BuildViewModel.UseNeuronalStimulator.ToString());

            saveBuilder.AppendLine(BuildViewModel.Strength.ToString());
            saveBuilder.AppendLine(BuildViewModel.Stamina.ToString());
            saveBuilder.AppendLine(BuildViewModel.Sense.ToString());
            saveBuilder.AppendLine(BuildViewModel.Agility.ToString());
            saveBuilder.AppendLine(BuildViewModel.Intelligence.ToString());
            saveBuilder.AppendLine(BuildViewModel.Psychic.ToString());
            saveBuilder.AppendLine(BuildViewModel.Treatment.ToString());

            void SaveLadderViewModel(LadderViewModel ladderViewModel)
            {
                saveBuilder.AppendLine(ladderViewModel.LadderStepViewModels.Count.ToString());
                foreach (var ladderStepViewModel in ladderViewModel.LadderStepViewModels)
                {
                    saveBuilder.AppendLine(ladderStepViewModel.Implant.AOID.ToString());
                    saveBuilder.AppendLine(ladderStepViewModel.Implant.QL.ToString());
                    saveBuilder.AppendLine(ladderStepViewModel.IsChecked.ToString());
                    saveBuilder.AppendLine(ladderStepViewModel.IsFinalImplant.ToString());
                }
                saveBuilder.AppendLine(ladderViewModel.AverageFinalImplantQL.ToString());
            }

            void SaveClusterPurchases(IReadOnlyList<ClusterPurchaseViewModel> clusterPurchases)
            {
                saveBuilder.AppendLine(clusterPurchases.Count.ToString());
                foreach (var clusterPurchase in clusterPurchases)
                {
                    saveBuilder.AppendLine(clusterPurchase.Stat.Name);
                    saveBuilder.AppendLine(clusterPurchase.MinimumClusterQL.ToString());
                    saveBuilder.AppendLine(clusterPurchase.IsChecked.ToString());
                }
            }

            void SaveShoppingViewModel(ShoppingViewModel shoppingViewModel)
            {
                SaveClusterPurchases(shoppingViewModel.ShinyClusterPurchases);
                SaveClusterPurchases(shoppingViewModel.BrightClusterPurchases);
                SaveClusterPurchases(shoppingViewModel.FadedClusterPurchases);
            }

            SaveLadderViewModel(BasicLadderViewModel);
            SaveShoppingViewModel(BasicShoppingViewModel);
            SaveLadderViewModel(AdvancedLadderViewModel);
            SaveShoppingViewModel(AdvancedShoppingViewModel);

            return saveBuilder.ToString();
        }

        public void LoadSave(string save)
        {
            using (var saveReader = new StringReader(save))
            {
                foreach (var implantConfigurationViewModel in BuildViewModel.ImplantConfigurationViewModels)
                {
                    if (int.TryParse(saveReader.ReadLine(), out int aoid))
                    {
                        implantConfigurationViewModel.SetImplantTemplate(ImplantTemplate.ImplantTemplates.Single(t => t.AOID == aoid));
                    }
                    else
                    {
                        implantConfigurationViewModel.SetImplantTemplate(null);
                    }
                }

                BuildViewModel.UseComposites = bool.Parse(saveReader.ReadLine());
                BuildViewModel.UseCompositesPay2Win = bool.Parse(saveReader.ReadLine());
                BuildViewModel.UseSurgeryClinicEffect = bool.Parse(saveReader.ReadLine());
                BuildViewModel.UseSuperiorFirstAid = bool.Parse(saveReader.ReadLine());
                BuildViewModel.UseEnhancedSenses = bool.Parse(saveReader.ReadLine());
                BuildViewModel.UseFelineGrace = bool.Parse(saveReader.ReadLine());
                BuildViewModel.UseEssenceOfBehemoth = bool.Parse(saveReader.ReadLine());
                BuildViewModel.UseGauntlet = bool.Parse(saveReader.ReadLine());
                BuildViewModel.UseExtruderBar = bool.Parse(saveReader.ReadLine());
                BuildViewModel.UseExplosifs = bool.Parse(saveReader.ReadLine());
                BuildViewModel.UseIronCircle = bool.Parse(saveReader.ReadLine());
                BuildViewModel.UseProdigiousStrength = bool.Parse(saveReader.ReadLine());
                BuildViewModel.UseIronCircleOrProdigiousStrength = bool.Parse(saveReader.ReadLine());
                BuildViewModel.UseNeuronalStimulator = bool.Parse(saveReader.ReadLine());

                BuildViewModel.Strength = int.Parse(saveReader.ReadLine());
                BuildViewModel.Stamina = int.Parse(saveReader.ReadLine());
                BuildViewModel.Sense = int.Parse(saveReader.ReadLine());
                BuildViewModel.Agility = int.Parse(saveReader.ReadLine());
                BuildViewModel.Intelligence = int.Parse(saveReader.ReadLine());
                BuildViewModel.Psychic = int.Parse(saveReader.ReadLine());
                BuildViewModel.Treatment = double.Parse(saveReader.ReadLine());

                void LoadLadderViewModel(LadderViewModel ladderViewModel)
                {
                    int ladderStepCount = int.Parse(saveReader.ReadLine());
                    var ladderStepViewModels = new List<LadderStepViewModel>();
                    for (int i = 1; i <= ladderStepCount; ++i)
                    {
                        int aoid = int.Parse(saveReader.ReadLine());
                        int implantQL = int.Parse(saveReader.ReadLine());
                        bool isChecked = bool.Parse(saveReader.ReadLine());
                        bool isFinalImplant = bool.Parse(saveReader.ReadLine());
                        var implant = new Implant(ImplantTemplate.ImplantTemplates.Single(t => t.AOID == aoid), implantQL);
                        ladderStepViewModels.Add(new LadderStepViewModel(implant, isFinalImplant)
                        {
                            IsChecked = isChecked
                        });
                    }
                    ladderViewModel.LadderStepViewModels = ladderStepViewModels;

                    if (double.TryParse(saveReader.ReadLine(), out double averageFinalImplantQL))
                    {
                        ladderViewModel.AverageFinalImplantQL = averageFinalImplantQL;
                    }
                    else
                    {
                        ladderViewModel.AverageFinalImplantQL = null;
                    }
                }

                IReadOnlyList<ClusterPurchaseViewModel> LoadClusterPurchases(IEnumerable<ClusterTemplate> clusterTemplates)
                {
                    int clusterPurchasesCount = int.Parse(saveReader.ReadLine());
                    var clusterPurchaseViewModels = new List<ClusterPurchaseViewModel>();
                    for (int i = 1; i <= clusterPurchasesCount; ++i)
                    {
                        string statName = saveReader.ReadLine();
                        int minimumClusterQL = int.Parse(saveReader.ReadLine());
                        bool isChecked = bool.Parse(saveReader.ReadLine());
                        var clusterTemplate = clusterTemplates.Single(t => t.Stat.Name == statName);
                        clusterPurchaseViewModels.Add(new ClusterPurchaseViewModel(clusterTemplate, minimumClusterQL)
                        {
                            IsChecked = isChecked
                        });
                    }

                    return clusterPurchaseViewModels;
                }

                void LoadShoppingViewModel(ShoppingViewModel shoppingViewModel)
                {
                    shoppingViewModel.ShinyClusterPurchases = LoadClusterPurchases(ShinyClusterTemplate.ShinyClusterTemplates);
                    shoppingViewModel.BrightClusterPurchases = LoadClusterPurchases(BrightClusterTemplate.BrightClusterTemplates);
                    shoppingViewModel.FadedClusterPurchases = LoadClusterPurchases(FadedClusterTemplate.FadedClusterTemplates);
                }

                LoadLadderViewModel(BasicLadderViewModel);
                LoadShoppingViewModel(BasicShoppingViewModel);
                LoadLadderViewModel(AdvancedLadderViewModel);
                LoadShoppingViewModel(AdvancedShoppingViewModel);
            }
        }

        public ICommand ExportBasicLadderToAuno { get; }
        public void ExecuteExportBasicLadderToAuno()
            => ExportToAuno(BasicLadderViewModel);

        public ICommand ExportAdvancedLadderToAuno { get; }
        public void ExecuteExportAdvancedLadderToAuno()
            => ExportToAuno(AdvancedLadderViewModel);

        // Could be exporting after loading a file, meaning we wouldn't have the ladder process object built.
        // That's why it's necessary to save/load IsFinalImplant.
        private void ExportToAuno(LadderViewModel ladderViewModel)
        {
            string baseUrl = "http://auno.org/ao/equip.php?noedit=1&";
            string filter = string.Join("&", ladderViewModel.LadderStepViewModels
                .Where(s => s.IsFinalImplant)
                .Select(s => $"id3-{s.ImplantSlot.AunoSlotID}={s.ImplantTemplate.AOID}&ql3-{s.ImplantSlot.AunoSlotID}={s.ImplantQL}"));
            string url = baseUrl + filter;
            Process.Start(url);
        }

        private void BuildViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BuildViewModel.BasicLadderProcess))
            {
                BasicLadderViewModel.LadderProcess = BuildViewModel.BasicLadderProcess;
                BasicShoppingViewModel.LadderProcess = BuildViewModel.BasicLadderProcess;
                SelectedTabIndex = 1;
            }
            else if (e.PropertyName == nameof(BuildViewModel.AdvancedLadderProcess))
            {
                AdvancedLadderViewModel.LadderProcess = BuildViewModel.AdvancedLadderProcess;
                AdvancedShoppingViewModel.LadderProcess = BuildViewModel.AdvancedLadderProcess;
                SelectedTabIndex = 3;
            }
        }
    }
}
