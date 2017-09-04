using AOLadderer.LadderProcesses;
using AOLadderer.Stats;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AOLadderer.UI.ViewModels
{
    public class BuildViewModel : ViewModelBase
    {
        public BuildViewModel()
        {
            ImplantConfigurationViewModels = ImplantSlot.ImplantSlots
                .Select(s => new ImplantConfigurationViewModel(s))
                .ToArray();
            RunBasicLaddererCommand = new RelayCommand(ExecuteRunBasicLaddererCommand);
            RunAdvancedLaddererCommand = new RelayCommand(ExecuteRunAdvancedLaddererCommand);
        }

        public IReadOnlyList<ImplantConfigurationViewModel> ImplantConfigurationViewModels { get; }

        private bool _useComposites;
        public bool UseComposites
        {
            get => _useComposites;
            set
            {
                if (Set(ref _useComposites, value))
                {
                    if (UseComposites)
                    {
                        UseCompositesPay2Win = false;
                        UpdateAbilities(12);
                        Treatment += 20;
                    }
                    else
                    {
                        UpdateAbilities(-12);
                        Treatment -= 20;
                    }
                }
            }
        }

        private bool _useCompositesPay2Win;
        public bool UseCompositesPay2Win
        {
            get => _useCompositesPay2Win;
            set
            {
                if (Set(ref _useCompositesPay2Win, value))
                {
                    if (UseCompositesPay2Win)
                    {
                        UseComposites = false;
                        UpdateAbilities(20);
                        Treatment += 35;
                    }
                    else
                    {
                        UpdateAbilities(-20);
                        Treatment -= 35;
                    }
                }
            }
        }

        private bool _useSurgeryClinicEffect;
        public bool UseSurgeryClinicEffect
        {
            get => _useSurgeryClinicEffect;
            set
            {
                if (Set(ref _useSurgeryClinicEffect, value))
                {
                    if (UseSurgeryClinicEffect)
                    {
                        Treatment += 100;
                    }
                    else
                    {
                        Treatment -= 100;
                    }
                }
            }
        }

        private bool _useSuperiorFirstAid;
        public bool UseSuperiorFirstAid
        {
            get => _useSuperiorFirstAid;
            set
            {
                if (Set(ref _useSuperiorFirstAid, value))
                {
                    if (UseSuperiorFirstAid)
                    {
                        Treatment += 80;
                    }
                    else
                    {
                        Treatment -= 80;
                    }
                }
            }
        }

        private bool _useEnhancedSenses;
        public bool UseEnhancedSenses
        {
            get => _useEnhancedSenses;
            set
            {
                if (Set(ref _useEnhancedSenses, value))
                {
                    if (UseEnhancedSenses)
                    {
                        Sense += 15;
                        Treatment += Ability.Sense.GetTreatmentTrickle(15);
                    }
                    else
                    {
                        Sense -= 15;
                        Treatment -= Ability.Sense.GetTreatmentTrickle(15);
                    }
                }
            }
        }

        private bool _useFelineGrace;
        public bool UseFelineGrace
        {
            get => _useFelineGrace;
            set
            {
                if (Set(ref _useFelineGrace, value))
                {
                    if (UseFelineGrace)
                    {
                        Agility += 25;
                        Treatment += Ability.Agility.GetTreatmentTrickle(25);
                    }
                    else
                    {
                        Agility -= 25;
                        Treatment -= Ability.Agility.GetTreatmentTrickle(25);
                    }
                }
            }
        }

        private bool _useEssenceOfBehemoth;
        public bool UseEssenceOfBehemoth
        {
            get => _useEssenceOfBehemoth;
            set
            {
                if (Set(ref _useEssenceOfBehemoth, value))
                {
                    if (UseEssenceOfBehemoth)
                    {
                        Strength += 27;
                        Stamina += 27;
                    }
                    else
                    {
                        Strength -= 27;
                        Stamina -= 27;
                    }
                }
            }
        }

        private bool _useGauntlet;
        public bool UseGauntlet
        {
            get => _useGauntlet;
            set
            {
                if (Set(ref _useGauntlet, value))
                {
                    if (UseGauntlet)
                    {
                        UpdateAbilities(10);
                    }
                    else
                    {
                        UpdateAbilities(-10);
                    }
                }
            }
        }

        private bool _useExtruderBar;
        public bool UseExtruderBar
        {
            get => _useExtruderBar;
            set
            {
                if (Set(ref _useExtruderBar, value))
                {
                    if (UseExtruderBar)
                    {
                        UpdateAbilities(2);
                    }
                    else
                    {
                        UpdateAbilities(-2);
                    }
                }
            }
        }

        private bool _useExplosifs;
        public bool UseExplosifs
        {
            get => _useExplosifs;
            set
            {
                if (Set(ref _useExplosifs, value))
                {
                    if (UseExplosifs)
                    {
                        UpdateAbilities(20, excludeTreatmentTrickle: true);
                    }
                    else
                    {
                        UpdateAbilities(-20, excludeTreatmentTrickle: true);
                    }
                }
            }
        }

        private bool _useIronCircle;
        public bool UseIronCircle
        {
            get => _useIronCircle;
            set
            {
                if (Set(ref _useIronCircle, value))
                {
                    if (UseIronCircle)
                    {
                        UseProdigiousStrength = UseIronCircleOrProdigiousStrength = false;
                        Strength += 20;
                        Stamina += 20;
                    }
                    else
                    {
                        Strength -= 20;
                        Stamina -= 20;
                    }
                }
            }
        }

        private bool _useProdigiousStrength;
        public bool UseProdigiousStrength
        {
            get => _useProdigiousStrength;
            set
            {
                if (Set(ref _useProdigiousStrength, value))
                {
                    if (UseProdigiousStrength)
                    {
                        UseIronCircle = UseIronCircleOrProdigiousStrength = false;
                        Strength += 40;
                    }
                    else
                    {
                        Strength -= 40;
                    }
                }
            }
        }

        private bool _useIronCircleOrProdigiousStrength;
        public bool UseIronCircleOrProdigiousStrength
        {
            get => _useIronCircleOrProdigiousStrength;
            set
            {
                if (Set(ref _useIronCircleOrProdigiousStrength, value))
                {
                    if (UseIronCircleOrProdigiousStrength)
                    {
                        UseIronCircle = UseProdigiousStrength = false;
                        Strength += 40;
                        Stamina += 20;
                    }
                    else
                    {
                        Strength -= 40;
                        Stamina -= 20;
                    }
                }
            }
        }

        private bool _useNeuronalStimulator;
        public bool UseNeuronalStimulator
        {
            get => _useNeuronalStimulator;
            set
            {
                if (Set(ref _useNeuronalStimulator, value))
                {
                    if (UseNeuronalStimulator)
                    {
                        Intelligence += 23;
                        Psychic += 23;
                        Treatment += Ability.Intelligence.GetTreatmentTrickle(23);
                    }
                    else
                    {
                        Intelligence -= 23;
                        Psychic -= 23;
                        Treatment -= Ability.Intelligence.GetTreatmentTrickle(23);
                    }
                }
            }
        }

        private int _strength;
        public int Strength
        {
            get => _strength;
            set => Set(ref _strength, value);
        }

        private int _stamina;
        public int Stamina
        {
            get => _stamina;
            set => Set(ref _stamina, value);
        }

        private int _sense;
        public int Sense
        {
            get => _sense;
            set => Set(ref _sense, value);
        }

        private int _agility;
        public int Agility
        {
            get => _agility;
            set => Set(ref _agility, value);
        }

        private int _intelligence;
        public int Intelligence
        {
            get => _intelligence;
            set => Set(ref _intelligence, value);
        }

        private int _psychic;
        public int Psychic
        {
            get => _psychic;
            set => Set(ref _psychic, value);
        }

        private double _treatment;
        public double Treatment
        {
            get => _treatment;
            set => Set(ref _treatment, value);
        }

        private void UpdateAbilities(int delta, bool excludeTreatmentTrickle = false)
        {
            Strength += delta;
            Stamina += delta;
            Sense += delta;
            Agility += delta;
            Intelligence += delta;
            Psychic += delta;

            if (!excludeTreatmentTrickle)
            {
                Treatment += Ability.Sense.GetTreatmentTrickle(delta);
                Treatment += Ability.Agility.GetTreatmentTrickle(delta);
                Treatment += Ability.Intelligence.GetTreatmentTrickle(delta);
            }
        }

        private Character GetCharacter()
            => new Character(Agility, Intelligence, Psychic, Sense, Stamina, Strength, Treatment);

        private IEnumerable<ImplantTemplate> GetImplantTemplates()
            => ImplantConfigurationViewModels
            .Where(i => !i.IsEmpty && i.IsImplantSlotAvailable)
            .Select(i => i.GetImplantTemplate());

        private IEnumerable<ImplantSlot> GetUnavailableImplantSlots()
            => ImplantConfigurationViewModels
            .Where(i => !i.IsImplantSlotAvailable)
            .Select(i => i.ImplantSlot);

        private bool _isLaddererRunning;
        public bool IsLaddererRunning
        {
            get => _isLaddererRunning;
            private set => Set(ref _isLaddererRunning, value);
        }

        private BasicLadderProcess _basicLadderProcess;
        public BasicLadderProcess BasicLadderProcess
        {
            get => _basicLadderProcess;
            private set => Set(ref _basicLadderProcess, value);
        }

        private AdvancedLadderProcess _advancedLadderProcess;
        public AdvancedLadderProcess AdvancedLadderProcess
        {
            get => _advancedLadderProcess;
            private set => Set(ref _advancedLadderProcess, value);
        }

        public ICommand RunBasicLaddererCommand { get; }
        private async void ExecuteRunBasicLaddererCommand()
        {
            var character = GetCharacter();
            var implantTemplates = GetImplantTemplates();
            var unavailableImplantSlots = GetUnavailableImplantSlots();

            IsLaddererRunning = true;
            BasicLadderProcess = await Task.Run(()
                => new BasicLadderProcess(character, implantTemplates, unavailableImplantSlots));
            IsLaddererRunning = false;
        }

        public ICommand RunAdvancedLaddererCommand { get; }
        private async void ExecuteRunAdvancedLaddererCommand()
        {
            var character = GetCharacter();
            var implantTemplates = GetImplantTemplates();
            var unavailableImplantSlots = GetUnavailableImplantSlots();

            IsLaddererRunning = true;
            AdvancedLadderProcess = await Task.Run(()
                => new AdvancedLadderProcess(character, implantTemplates, unavailableImplantSlots));
            IsLaddererRunning = false;
        }
    }
}
