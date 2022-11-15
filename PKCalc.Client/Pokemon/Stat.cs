using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKCalc.Client.Pokemon
{
    public class Stat
    {
        public StatEnum Id { get; }
        public string? Name { get; }
        public bool IsNotHP => !this.Id.Equals(StatEnum.HP);
        private int _calculated;
        public int Calculated
        {
            get => this._calculated;
            set
            {
                this._calculated = value;
                if (this.Id == StatEnum.HP)
                    MaxHealthChanged?.Invoke(this, value);
            }
        }
        public int Total { get; set; }
        public event EventHandler<int> MaxHealthChanged;

        private int _baseStat;
        public int BaseStat
        {
            get => _baseStat;
            set
            {
                if (value < 0) value = 0;
                else if (value > 255) value = 255;
                _baseStat = value;
                Calculated = Calculate();
                Total = CalculateTotal();
            }
        }
        private int _level;
        public int Level
        {
            get => _level;
            set
            {
                if (value < 1) value = 1;
                else if (value > 100) value = 100;
                _level = value;
                Calculated = Calculate();
                Total = CalculateTotal();
            }
        }
        private float _natureModifier;
        public float NatureModifier
        {
            get => _natureModifier;
            set
            {
                if (value < 0.8f) value = 0.8f;
                else if (value > 1.2f) value = 1.2f;
                _natureModifier = value;
                Calculated = Calculate();
                Total = CalculateTotal();
            }
        }
        private int _ivs;
        public int IVs
        {
            get => _ivs;
            set
            {
                if (value < 0) value = 0;
                else if (value > 31) value = 31;
                _ivs = value;
                Calculated = Calculate();
                Total = CalculateTotal();
            }
        }
        private int _evs;
        public int EVs
        {
            get => _evs;
            set
            {
                if (value < 0) value = 0;
                else if (value > 252) value = 252;
                _evs = value;
                Calculated = Calculate();
                Total = CalculateTotal();
            }
        }
        private int _boostModifier;
        public int BoostModifier
        {
            get => _boostModifier;
            set
            {
                if (value < -6) value = -6;
                else if (value > 6) value = 6;
                _boostModifier = value;
                Total = CalculateTotal();
            }
        }
        private float _fieldModifier;
        public float FieldModifier
        {
            get => _fieldModifier;
            set
            {
                if (value < 0.5f) value = 0.5f;
                else if (value > 2f) value = 2f;
                _fieldModifier = value;
                Total = CalculateTotal();
            }
        }
        private float _abilityModifier;
        public float AbilityModifier
        {
            get => _abilityModifier;
            set
            {
                if (value < 0.5f) value = 0.5f;
                else if (value > 1.5f) value = 1.5f;
                _abilityModifier = value;
                Total = CalculateTotal();
            }
        }
        private float _itemModifier;
        public float ItemModifier
        {
            get => _itemModifier;
            set
            {
                if (value < 0.5f) value = 0.5f;
                else if (value > 1.5f) value = 1.5f;
                _itemModifier = value;
                Total = CalculateTotal();
            }
        }


        public Stat(
            StatEnum id,
            int baseStat,
            int level = 50,
            float natureModifier = 1,
            int ivs = 31,
            int evs = 0,
            int boostModifier = 0,
            float fieldModifier = 1,
            float abilityModifier = 1,
            float itemModifier = 1)
        {
            Id = id;
            Name = Id switch
            {
                StatEnum.None => "",
                StatEnum.HP => "HP",
                StatEnum.Attack => "Attack",
                StatEnum.Defense => "Defense",
                StatEnum.SpecialAttack => "Sp. Atk",
                StatEnum.SpecialDefense => "Sp. Def",
                StatEnum.Speed => "Speed",
                _ => throw new ArgumentException("Invalid stat id"),
            };
            BaseStat = baseStat;
            Level = level;
            NatureModifier = natureModifier;
            IVs = ivs;
            EVs = evs;
            Calculated = Calculate();
            BoostModifier = boostModifier;
            FieldModifier = fieldModifier;
            AbilityModifier = abilityModifier;
            ItemModifier = itemModifier;
            Total = CalculateTotal();
        }

        public int Calculate()
        {
            if (this.IsNotHP)
            {
                return Convert.ToInt32(Math.Floor(((((2 * BaseStat) + IVs + (EVs / 4)) * Level / 100) + 5f) * NatureModifier));
            }
            else
            {
                if (this.BaseStat == 1) return 1;
                return (((2 * BaseStat) + IVs + (EVs / 4)) * Level / 100) + Level + 10;
            }
        }

        public int CalculateTotal()
        {
            if (this.IsNotHP)
            {
                int result = 0;
                // Calculate Stat Boosts
                if (this.BoostModifier > 0) result = Convert.ToInt32(Math.Floor(this.Calculated * ((2 + this.BoostModifier) / 2d)));
                else if (BoostModifier < 0) result = Convert.ToInt32(Math.Floor(this.Calculated * (2d / (2 - this.BoostModifier))));
                else result = this.Calculated;
                // Calculate other modifiers
                result = Convert.ToInt32(Math.Floor(result * this.FieldModifier * this.ItemModifier * this.AbilityModifier));
                return result;
            }
            else
            {
                return this.Calculated;
            }
        }

    }
}
