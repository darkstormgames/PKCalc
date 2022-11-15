using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKCalc.Client.Pokemon
{
    public class EggGroup
    {
        private readonly PokemonService _service;
        
        public EggGroupEnum Id { get; private set; }
        public string Name { get; private set; }

        private EggGroup(PokemonService service)
        {
            this._service = service;
        }

        public static IEnumerable<EggGroup> GetEggGroups(PokemonService service)
        {
            PokemonService.Instance.Logger.Debug("Getting egg groups...");
            return new List<EggGroup>
            {
                new EggGroup(service) { Id = EggGroupEnum.Undiscovered, Name = "Undiscovered" },
                new EggGroup(service) { Id = EggGroupEnum.Bug, Name = "Bug" },
                new EggGroup(service) { Id = EggGroupEnum.Ditto, Name = "Ditto" },
                new EggGroup(service) { Id = EggGroupEnum.Dragon, Name = "Dragon" },
                new EggGroup(service) { Id = EggGroupEnum.Fairy, Name = "Fairy" },
                new EggGroup(service) { Id = EggGroupEnum.Field, Name = "Field" },
                new EggGroup(service) { Id = EggGroupEnum.Flying, Name = "Flying" },
                new EggGroup(service) { Id = EggGroupEnum.Grass, Name = "Grass" },
                new EggGroup(service) { Id = EggGroupEnum.HumanLike, Name = "Human-Like" },
                new EggGroup(service) { Id = EggGroupEnum.Amorphous, Name = "Amorphous" },
                new EggGroup(service) { Id = EggGroupEnum.Mineral, Name = "Mineral" },
                new EggGroup(service) { Id = EggGroupEnum.Monster, Name = "Monster" },
                new EggGroup(service) { Id = EggGroupEnum.Water1, Name = "Water 1" },
                new EggGroup(service) { Id = EggGroupEnum.Water2, Name = "Water 2" },
                new EggGroup(service) { Id = EggGroupEnum.Water3, Name = "Water 3" },
            };
        }
    }
}
