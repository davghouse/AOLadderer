using AOLadderer.ClusterTemplates;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace AOLadderer.UI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
            => BuildViewModel.PropertyChanged += BuildViewModel_PropertyChanged;

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
        // manual effort. But I don't want our serialization to affect the AOLadderer project.
        public void SaveToFile(string filePath)
        {
            using (var file = new StreamWriter(filePath))
            {
                foreach (var implantConfigurationViewModel in BuildViewModel.ImplantConfigurationViewModels)
                {
                    file.WriteLine(implantConfigurationViewModel.GetImplantTemplate()?.AOID);
                }

                file.WriteLine(BuildViewModel.UseComposites);
                file.WriteLine(BuildViewModel.UseCompositesPay2Win);
                file.WriteLine(BuildViewModel.UseSurgeryClinicEffect);
                file.WriteLine(BuildViewModel.UseSuperiorFirstAid);
                file.WriteLine(BuildViewModel.UseEnhancedSenses);
                file.WriteLine(BuildViewModel.UseFelineGrace);
                file.WriteLine(BuildViewModel.UseEssenceOfBehemoth);
                file.WriteLine(BuildViewModel.UseGauntlet);
                file.WriteLine(BuildViewModel.UseExtruderBar);
                file.WriteLine(BuildViewModel.UseExplosifs);
                file.WriteLine(BuildViewModel.UseIronCircle);
                file.WriteLine(BuildViewModel.UseProdigiousStrength);
                file.WriteLine(BuildViewModel.UseIronCircleOrProdigiousStrength);
                file.WriteLine(BuildViewModel.UseNeuronalStimulator);

                file.WriteLine(BuildViewModel.Strength);
                file.WriteLine(BuildViewModel.Stamina);
                file.WriteLine(BuildViewModel.Sense);
                file.WriteLine(BuildViewModel.Agility);
                file.WriteLine(BuildViewModel.Intelligence);
                file.WriteLine(BuildViewModel.Psychic);
                file.WriteLine(BuildViewModel.Treatment);

                void SaveLadderViewModelToFile(LadderViewModel ladderViewModel)
                {
                    file.WriteLine(ladderViewModel.LadderStepViewModels.Count);
                    foreach (var ladderStepViewModel in ladderViewModel.LadderStepViewModels)
                    {
                        file.WriteLine(ladderStepViewModel.Implant.AOID);
                        file.WriteLine(ladderStepViewModel.Implant.QL);
                        file.WriteLine(ladderStepViewModel.IsChecked);
                    }
                    file.WriteLine(ladderViewModel.AverageFinalImplantQL);
                }

                void SaveClusterPurchasesToFile(IReadOnlyList<ClusterPurchaseViewModel> clusterPurchases)
                {
                    file.WriteLine(clusterPurchases.Count);
                    foreach (var clusterPurchase in clusterPurchases)
                    {
                        file.WriteLine(clusterPurchase.Stat.Name);
                        file.WriteLine(clusterPurchase.MinimumClusterQL);
                        file.WriteLine(clusterPurchase.IsChecked);
                    }
                }

                void SaveShoppingViewModelToFile(ShoppingViewModel shoppingViewModel)
                {
                    SaveClusterPurchasesToFile(shoppingViewModel.ShinyClusterPurchases);
                    SaveClusterPurchasesToFile(shoppingViewModel.BrightClusterPurchases);
                    SaveClusterPurchasesToFile(shoppingViewModel.FadedClusterPurchases);
                }

                SaveLadderViewModelToFile(BasicLadderViewModel);
                SaveShoppingViewModelToFile(BasicShoppingViewModel);
                SaveLadderViewModelToFile(AdvancedLadderViewModel);
                SaveShoppingViewModelToFile(AdvancedShoppingViewModel);
            }
        }

        public void LoadFromFile(string filePath)
        {
            using (var file = new StreamReader(filePath))
            {
                foreach (var implantConfigurationViewModel in BuildViewModel.ImplantConfigurationViewModels)
                {
                    if (int.TryParse(file.ReadLine(), out int aoid))
                    {
                        implantConfigurationViewModel.SetImplantTemplate(ImplantTemplate.ImplantTemplates.Single(t => t.AOID == aoid));
                    }
                    else
                    {
                        implantConfigurationViewModel.SetImplantTemplate(null);
                    }
                }

                BuildViewModel.UseComposites = bool.Parse(file.ReadLine());
                BuildViewModel.UseCompositesPay2Win = bool.Parse(file.ReadLine());
                BuildViewModel.UseSurgeryClinicEffect = bool.Parse(file.ReadLine());
                BuildViewModel.UseSuperiorFirstAid = bool.Parse(file.ReadLine());
                BuildViewModel.UseEnhancedSenses = bool.Parse(file.ReadLine());
                BuildViewModel.UseFelineGrace = bool.Parse(file.ReadLine());
                BuildViewModel.UseEssenceOfBehemoth = bool.Parse(file.ReadLine());
                BuildViewModel.UseGauntlet = bool.Parse(file.ReadLine());
                BuildViewModel.UseExtruderBar = bool.Parse(file.ReadLine());
                BuildViewModel.UseExplosifs = bool.Parse(file.ReadLine());
                BuildViewModel.UseIronCircle = bool.Parse(file.ReadLine());
                BuildViewModel.UseProdigiousStrength = bool.Parse(file.ReadLine());
                BuildViewModel.UseIronCircleOrProdigiousStrength = bool.Parse(file.ReadLine());
                BuildViewModel.UseNeuronalStimulator = bool.Parse(file.ReadLine());

                BuildViewModel.Strength = int.Parse(file.ReadLine());
                BuildViewModel.Stamina = int.Parse(file.ReadLine());
                BuildViewModel.Sense = int.Parse(file.ReadLine());
                BuildViewModel.Agility = int.Parse(file.ReadLine());
                BuildViewModel.Intelligence = int.Parse(file.ReadLine());
                BuildViewModel.Psychic = int.Parse(file.ReadLine());
                BuildViewModel.Treatment = double.Parse(file.ReadLine());

                void LoadLadderViewModelFromFile(LadderViewModel ladderViewModel)
                {
                    int ladderStepCount = int.Parse(file.ReadLine());
                    var ladderStepViewModels = new List<LadderStepViewModel>();
                    for (int i = 1; i <= ladderStepCount; ++i)
                    {
                        int aoid = int.Parse(file.ReadLine());
                        int implantQL = int.Parse(file.ReadLine());
                        bool isChecked = bool.Parse(file.ReadLine());
                        var implant = new Implant(ImplantTemplate.ImplantTemplates.Single(t => t.AOID == aoid), implantQL);
                        ladderStepViewModels.Add(new LadderStepViewModel(implant)
                            {
                                IsChecked = isChecked
                            });
                    }
                    ladderViewModel.LadderStepViewModels = ladderStepViewModels;

                    if (double.TryParse(file.ReadLine(), out double averageFinalImplantQL))
                    {
                        ladderViewModel.AverageFinalImplantQL = averageFinalImplantQL;
                    }
                    else
                    {
                        ladderViewModel.AverageFinalImplantQL = null;
                    }
                }

                IReadOnlyList<ClusterPurchaseViewModel> LoadClusterPurchasesFromFile(IEnumerable<ClusterTemplate> clusterTemplates)
                {
                    int clusterPurchasesCount = int.Parse(file.ReadLine());
                    var clusterPurchaseViewModels = new List<ClusterPurchaseViewModel>();
                    for (int i = 1; i <= clusterPurchasesCount; ++i)
                    {
                        string statName = file.ReadLine();
                        int minimumClusterQL = int.Parse(file.ReadLine());
                        bool isChecked = bool.Parse(file.ReadLine());
                        var clusterTemplate = clusterTemplates.Single(t => t.Stat.Name == statName);
                        clusterPurchaseViewModels.Add(new ClusterPurchaseViewModel(clusterTemplate, minimumClusterQL)
                            {
                                IsChecked = isChecked
                            });
                    }

                    return clusterPurchaseViewModels;
                }

                void LoadShoppingViewModelFromFile(ShoppingViewModel shoppingViewModel)
                {
                    shoppingViewModel.ShinyClusterPurchases = LoadClusterPurchasesFromFile(ShinyClusterTemplate.ShinyClusterTemplates);
                    shoppingViewModel.BrightClusterPurchases = LoadClusterPurchasesFromFile(BrightClusterTemplate.BrightClusterTemplates);
                    shoppingViewModel.FadedClusterPurchases = LoadClusterPurchasesFromFile(FadedClusterTemplate.FadedClusterTemplates);
                }

                LoadLadderViewModelFromFile(BasicLadderViewModel);
                LoadShoppingViewModelFromFile(BasicShoppingViewModel);
                LoadLadderViewModelFromFile(AdvancedLadderViewModel);
                LoadShoppingViewModelFromFile(AdvancedShoppingViewModel);
            }
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
