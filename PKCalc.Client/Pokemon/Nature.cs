using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKCalc.Client.Pokemon
{
    public class Nature
    {
        private readonly PokemonService _service;
        
        public NatureEnum Id { get; private set; }
        public string Name { get; private set; }
        public StatEnum UpStat { get; private set; }
        public string UpStatDisplayName => UpStat == StatEnum.None ? "" : getShortStatString(this.UpStat) + " 🠕";
        public StatEnum DownStat { get; private set; }
        public string DownStatDisplayName => DownStat == StatEnum.None ? "" : getShortStatString(this.DownStat) + " 🠗";
        
        private Nature(PokemonService service)
        {
            this._service = service;
        }

        public static IEnumerable<Nature> GetNatures(PokemonService service)
        {
            PokemonService.Instance.Logger.Debug("Getting natures...");
            return new List<Nature>
            {
                new Nature(service) { Id = NatureEnum.Adamant, Name = "Adamant", UpStat = StatEnum.Attack, DownStat = StatEnum.SpecialAttack},
                new Nature(service) { Id = NatureEnum.Bashful, Name = "Bashful", UpStat = StatEnum.None, DownStat = StatEnum.None},
                new Nature(service) { Id = NatureEnum.Bold, Name = "Bold", UpStat = StatEnum.Defense, DownStat = StatEnum.Attack},
                new Nature(service) { Id = NatureEnum.Brave, Name = "Brave", UpStat = StatEnum.Attack, DownStat = StatEnum.Speed},
                new Nature(service) { Id = NatureEnum.Calm, Name = "Calm", UpStat = StatEnum.SpecialDefense, DownStat = StatEnum.Speed},
                new Nature(service) { Id = NatureEnum.Careful, Name = "Careful", UpStat = StatEnum.SpecialDefense, DownStat = StatEnum.SpecialAttack},
                new Nature(service) { Id = NatureEnum.Docile, Name = "Docile", UpStat = StatEnum.None, DownStat = StatEnum.None},
                new Nature(service) { Id = NatureEnum.Gentle, Name = "Gentle", UpStat = StatEnum.SpecialDefense, DownStat = StatEnum.Defense},
                new Nature(service) { Id = NatureEnum.Hardy, Name = "Hardy", UpStat = StatEnum.None, DownStat = StatEnum.None},
                new Nature(service) { Id = NatureEnum.Hasty, Name = "Hasty", UpStat = StatEnum.Speed, DownStat = StatEnum.Defense},
                new Nature(service) { Id = NatureEnum.Impish, Name = "Impish", UpStat = StatEnum.Defense, DownStat = StatEnum.SpecialAttack},
                new Nature(service) { Id = NatureEnum.Jolly, Name = "Jolly", UpStat = StatEnum.Speed, DownStat = StatEnum.SpecialAttack},
                new Nature(service) { Id = NatureEnum.Lax, Name = "Lax", UpStat = StatEnum.Defense, DownStat = StatEnum.SpecialDefense},
                new Nature(service) { Id = NatureEnum.Lonely, Name = "Lonely", UpStat = StatEnum.Attack, DownStat = StatEnum.Defense},
                new Nature(service) { Id = NatureEnum.Mild, Name = "Mild", UpStat = StatEnum.SpecialAttack, DownStat = StatEnum.Defense},
                new Nature(service) { Id = NatureEnum.Modest, Name = "Modest", UpStat = StatEnum.SpecialAttack, DownStat = StatEnum.Attack},
                new Nature(service) { Id = NatureEnum.Naive, Name = "Naive", UpStat = StatEnum.Speed, DownStat = StatEnum.SpecialDefense},
                new Nature(service) { Id = NatureEnum.Naughty, Name = "Naughty", UpStat = StatEnum.Attack, DownStat = StatEnum.SpecialDefense},
                new Nature(service) { Id = NatureEnum.Quiet, Name = "Quiet", UpStat = StatEnum.SpecialAttack, DownStat = StatEnum.Speed},
                new Nature(service) { Id = NatureEnum.Quirky, Name = "Quirky", UpStat = StatEnum.None, DownStat = StatEnum.None},
                new Nature(service) { Id = NatureEnum.Rash, Name = "Rash", UpStat = StatEnum.SpecialAttack, DownStat = StatEnum.SpecialDefense},
                new Nature(service) { Id = NatureEnum.Relaxed, Name = "Relaxed", UpStat = StatEnum.Defense, DownStat = StatEnum.Speed},
                new Nature(service) { Id = NatureEnum.Sassy, Name = "Sassy", UpStat = StatEnum.SpecialDefense, DownStat = StatEnum.Speed},
                new Nature(service) { Id = NatureEnum.Serious, Name = "Serious", UpStat = StatEnum.None, DownStat = StatEnum.None},
                new Nature(service) { Id = NatureEnum.Timid, Name = "Timid", UpStat = StatEnum.Speed, DownStat = StatEnum.Attack}
            };
        }

        private static string getShortStatString(StatEnum stat)
        {
            return stat switch
            {
                StatEnum.Attack => "Atk",
                StatEnum.Defense => "Def",
                StatEnum.SpecialAttack => "SpA",
                StatEnum.SpecialDefense => "SpD",
                StatEnum.Speed => "Spe",
                _ => "",
            };
        }
    }
}
