using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PKCalc.Client.Pokemon;
using PKCalc.UI;

namespace PKCalc.ViewModels.Pokemon
{
    internal class StatRowViewModel : ViewModelBase
    {
        public ICommand Values_ValueChanged { get; }
        public ICommand Values_LostFocus { get; }

        internal Stat Stat;

        public StatEnum StatEnum => this.Stat.Id;
        
        public string? StatName
        {
            get => this.Stat.Name;
        }

        public bool IsNotHP
        {
            get => !string.IsNullOrEmpty(this.StatName) && !this.StatName.Equals("HP");
        }

        public int BaseStat
        {
            get => this.Stat.BaseStat;
            set
            {
                this.Stat.BaseStat = value;
                this.UpdateStats();
            }
        }

        public float NatureModifier
        {
            get => this.Stat.NatureModifier;
            set
            {
                this.Stat.NatureModifier = value;
                this.UpdateStats();
            }
        }

        public int StatIVs
        {
            get => this.Stat.IVs;
            set
            {
                this.Stat.IVs = value;
                this.UpdateStats();
            }
        }

        public int StatEVs
        {
            get => this.Stat.EVs;
            set
            {
                this.Stat.EVs = value;
                this.UpdateStats();
            }
        }

        public string? BoostModifier
        {
            get
            {
                if (this.Stat.BoostModifier == 0) return "--";
                else
                {
                    return this.Stat.BoostModifier switch
                    {
                        1 => "+1",
                        2 => "+2",
                        3 => "+3",
                        4 => "+4",
                        5 => "+5",
                        6 => "+6",
                        -1 => "-1",
                        -2 => "-2",
                        -3 => "-3",
                        -4 => "-4",
                        -5 => "-5",
                        -6 => "-6",
                        _ => "--",
                    };
                }
            }
            set
            {
                this.Stat.BoostModifier = value switch
                {
                    "+1" => 1,
                    "+2" => 2,
                    "+3" => 3,
                    "+4" => 4,
                    "+5" => 5,
                    "+6" => 6,
                    "-1" => -1,
                    "-2" => -2,
                    "-3" => -3,
                    "-4" => -4,
                    "-5" => -5,
                    "-6" => -6,
                    _ => 0,
                };
                this.UpdateStats();
            }
        }

        public float FieldModifier
        {
            get => this.Stat.FieldModifier;
        }

        public int StatCalculated => this.Stat.Calculated;
        public int StatTotal => this.Stat.Total;

        public StatRowViewModel()
        {
            this.Stat = new(StatEnum.None, 5);
            Values_ValueChanged = new ActionCommand<RoutedPropertyChangedEventArgs<double?>>(e => UpdateStats());
            Values_LostFocus = new ActionCommand<RoutedEventArgs>(e => UpdateStats());
        }

        public void SetStats(Stat stat)
        {
            this.Stat = stat;

            base.OnPropertyChanged(nameof(StatEnum));
            base.OnPropertyChanged(nameof(StatName));
            base.OnPropertyChanged(nameof(IsNotHP));
            this.UpdateStats();
        }

        public void UpdateStats()
        {
            this.OnPropertyChanged(nameof(BaseStat));
            this.OnPropertyChanged(nameof(StatIVs));
            this.OnPropertyChanged(nameof(StatEVs));
            this.OnPropertyChanged(nameof(BoostModifier));
            this.OnPropertyChanged(nameof(NatureModifier));
            this.OnPropertyChanged(nameof(StatCalculated));
            this.OnPropertyChanged(nameof(StatTotal));
        }

        protected override void Cleanup()
        {
            
        }
    }
}
