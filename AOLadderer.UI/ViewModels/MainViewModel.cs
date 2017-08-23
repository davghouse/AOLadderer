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
        public ShoppingViewModel BasicShoppingViewModel = new ShoppingViewModel();
        public LadderViewModel AdvancedLadderViewModel { get; } = new LadderViewModel();
        public ShoppingViewModel AdvancedShoppingViewModel = new ShoppingViewModel();

        /* Erm, maybe we should just use [Serializable]? */

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

                int basicLadderStepCount = int.Parse(file.ReadLine());
                var basicLadderStepViewModels = new List<LadderStepViewModel>();
                for (int i = 1; i <= basicLadderStepCount; ++i)
                {
                    int aoid = int.Parse(file.ReadLine());
                    int implantQL = int.Parse(file.ReadLine());
                    bool isChecked = bool.Parse(file.ReadLine());
                    var implant = new Implant(ImplantTemplate.ImplantTemplates.Single(t => t.AOID == aoid), implantQL);
                    basicLadderStepViewModels.Add(new LadderStepViewModel(implant)
                    {
                        IsChecked = isChecked
                    });
                }
                BasicLadderViewModel.LadderStepViewModels = basicLadderStepViewModels;
                if (double.TryParse(file.ReadLine(), out double basicAverageFinalImplantQL))
                {
                    BasicLadderViewModel.AverageFinalImplantQL = basicAverageFinalImplantQL;
                }
                else
                {
                    BasicLadderViewModel.AverageFinalImplantQL = null;
                }

                int advancedLadderStepCount = int.Parse(file.ReadLine());
                var advancedLadderStepViewModels = new List<LadderStepViewModel>();
                for (int i = 1; i <= advancedLadderStepCount; ++i)
                {
                    int aoid = int.Parse(file.ReadLine());
                    int implantQL = int.Parse(file.ReadLine());
                    bool isChecked = bool.Parse(file.ReadLine());
                    var implant = new Implant(ImplantTemplate.ImplantTemplates.Single(t => t.AOID == aoid), implantQL);
                    advancedLadderStepViewModels.Add(new LadderStepViewModel(implant)
                    {
                        IsChecked = isChecked
                    });
                }
                AdvancedLadderViewModel.LadderStepViewModels = advancedLadderStepViewModels;
                if (double.TryParse(file.ReadLine(), out double advancedAverageFinalImplantQL))
                {
                    AdvancedLadderViewModel.AverageFinalImplantQL = advancedAverageFinalImplantQL;
                }
                else
                {
                    AdvancedLadderViewModel.AverageFinalImplantQL = null;
                }
            }
        }

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

                file.WriteLine(BasicLadderViewModel.LadderStepViewModels.Count);
                foreach (var ladderStepViewModel in BasicLadderViewModel.LadderStepViewModels)
                {
                    file.WriteLine(ladderStepViewModel.Implant.AOID);
                    file.WriteLine(ladderStepViewModel.Implant.QL);
                    file.WriteLine(ladderStepViewModel.IsChecked);
                }
                file.WriteLine(BasicLadderViewModel.AverageFinalImplantQL);

                file.WriteLine(AdvancedLadderViewModel.LadderStepViewModels.Count);
                foreach (var ladderStepViewModel in AdvancedLadderViewModel.LadderStepViewModels)
                {
                    file.WriteLine(ladderStepViewModel.Implant.AOID);
                    file.WriteLine(ladderStepViewModel.Implant.QL);
                    file.WriteLine(ladderStepViewModel.IsChecked);
                }
                file.WriteLine(AdvancedLadderViewModel.AverageFinalImplantQL);
            }
        }

        private void BuildViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BuildViewModel.BasicLadderProcess))
            {
                BasicLadderViewModel.LadderProcess = BuildViewModel.BasicLadderProcess;
            }
            else if (e.PropertyName == nameof(BuildViewModel.AdvancedLadderProcess))
            {
                AdvancedLadderViewModel.LadderProcess = BuildViewModel.AdvancedLadderProcess;
            }
        }
    }
}
