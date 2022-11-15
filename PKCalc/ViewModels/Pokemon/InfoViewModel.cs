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
    internal class InfoViewModel : ViewModelBase
    {
        private readonly Client.PokemonService _service = App.Service;
        public Client.PokemonService Service => this._service;
        
        private Views.Pokemon.InfoView _view;
        public ICommand View_Loaded { get; }

        private Dictionary<string, StatRowViewModel> statVMs = new();

        private Nature? _nature;
        public Nature? Nature
        {
            get => this._nature;
            set
            {
                if (value == null) value = App.Service.Natures.First();
                foreach (StatRowViewModel vm in this.statVMs.Values)
                {
                    if (vm.StatEnum == value.UpStat)
                        vm.NatureModifier = 1.1f;
                    else if (vm.StatEnum == value.DownStat)
                        vm.NatureModifier = 0.9f;
                    else
                        vm.NatureModifier = 1;
                }
                this.Set(ref this._nature, value);
            }
        }

        private Ability? _ability;
        public Ability? Ability
        {
            get => this._ability;
            set => this.Set(ref this._ability, value);
        }


        private int _currentHealth;
        public int CurrentHealth
        {
            get => this._currentHealth;
            set
            {
                this.Set(ref this._currentHealth, value);
                this._healthPercent = 100f * CurrentHealth / MaxHealth;
                this.OnPropertyChanged(nameof(HealthPercent));
            }
        }

        private int _maxHealth;
        public int MaxHealth
        {
            get => this._maxHealth;
            set => this.Set(ref this._maxHealth, value);
        }

        private float _healthPercent;
        public float HealthPercent
        {
            get => this._healthPercent;
            set
            {
                this.Set(ref this._healthPercent, (float)Math.Floor((double)value));
                this._currentHealth = (int)((this.HealthPercent * this.MaxHealth) / 100f);
                this.OnPropertyChanged(nameof(CurrentHealth));
            }
        }


        public InfoViewModel()
        {
            this.View_Loaded = new ActionCommand<RoutedEventArgs>(e =>
            {
                if (e.Source is Views.Pokemon.InfoView view && this.statVMs.Count == 0)
                {
                    this.loadViewModels(view);
                    this.loadDefaultData();
                }
            });
        }

        private void loadViewModels(Views.Pokemon.InfoView infoView)
        {
            this._view = infoView;
            if (this._view.statRowHP.DataContext is StatRowViewModel vmHP) statVMs.Add("HP", vmHP);
            if (this._view.statRowAtk.DataContext is StatRowViewModel vmAtk) statVMs.Add("Atk", vmAtk);
            if (this._view.statRowDef.DataContext is StatRowViewModel vmDef) statVMs.Add("Def", vmDef);
            if (this._view.statRowSpAtk.DataContext is StatRowViewModel vmSpAtk) statVMs.Add("SpA", vmSpAtk);
            if (this._view.statRowSpDef.DataContext is StatRowViewModel vmSpDef) statVMs.Add("SpD", vmSpDef);
            if (this._view.statRowSpe.DataContext is StatRowViewModel vmSpe) statVMs.Add("Spe", vmSpe);
        }

        private void loadDefaultData()
        {
            this.statVMs["HP"].SetStats(new Stat(StatEnum.HP, 80));
            this.statVMs["HP"].Stat.MaxHealthChanged += hpstat_MaxHealthChanged;
            this.HealthPercent = 100;
            this.statVMs["Atk"].SetStats(new Stat(StatEnum.Attack, 100));
            this.statVMs["Def"].SetStats(new Stat(StatEnum.Defense, 50));
            this.statVMs["SpA"].SetStats(new Stat(StatEnum.SpecialAttack, 100));
            this.statVMs["SpD"].SetStats(new Stat(StatEnum.SpecialDefense, 50));
            this.statVMs["Spe"].SetStats(new Stat(StatEnum.Speed, 200));
            this.Nature = App.Service.Natures.First();
        }

        private void hpstat_MaxHealthChanged(object? sender, int e)
        {
            this.MaxHealth = e;
            this._currentHealth = (int)((this.HealthPercent * this.MaxHealth) / 100f);
            this.OnPropertyChanged(nameof(CurrentHealth));
        }

        protected override void Cleanup()
        {
            
        }
    }
}
