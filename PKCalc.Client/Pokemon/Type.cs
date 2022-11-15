using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKCalc.Client.Pokemon
{
    public class Type
    {
        private readonly PokemonService _service;
        
        public TypeEnum Id { get; private set; }
        public string Name { get; private set; }
        public Dictionary<string, float> DamageTaken { get; private set; }

        private Type(PokemonService service)
        {
            this._service = service;
        }

        public static IEnumerable<Type> GetTypeChart(PokemonService service)
        {
            PokemonService.Instance.Logger.Debug("Getting type chart...");
            List<Type> typeChart = new()
            {
                new Type(service)
                {
                    Id = TypeEnum.None,
                    Name = "(none)",
                    DamageTaken = new()
                    {
                        { "(none)", 1 },
                        { "Bug", 1 },
                        { "Dark", 1 },
                        { "Dragon", 1 },
                        { "Electric", 1 },
                        { "Fairy", 1 },
                        { "Fighting", 1 },
                        { "Fire", 1 },
                        { "Flying", 1 },
                        { "Ghost", 1 },
                        { "Grass", 1 },
                        { "Ground", 1 },
                        { "Ice", 1 },
                        { "Normal", 1 },
                        { "Poison", 1 },
                        { "Psychic", 1 },
                        { "Rock", 1 },
                        { "Steel", 1 },
                        { "Water", 1 }
                    }
                },
                new Type(service)
                {
                    Id = TypeEnum.Bug,
                    Name = "Bug",
                    DamageTaken = new()
                    {
                        { "(none)", 1 },
                        { "Bug", 1 },
                        { "Dark", 2 },
                        { "Dragon", 1 },
                        { "Electric", 1 },
                        { "Fairy", 1 },
                        { "Fighting", .5f },
                        { "Fire", 2 },
                        { "Flying", 2 },
                        { "Ghost", 1 },
                        { "Grass", .5f },
                        { "Ground", .5f },
                        { "Ice", 1 },
                        { "Normal", 1 },
                        { "Poison", 1 },
                        { "Psychic", 1 },
                        { "Rock", 2 },
                        { "Steel", 1 },
                        { "Water", 1 }
                    }
                },
                new Type(service)
                {
                    Id = TypeEnum.Dark,
                    Name = "Dark",
                    DamageTaken = new()
                    {
                        { "(none)", 1 },
                        { "prankster", 0 },
                        { "Bug", 2 },
                        { "Dark", .5f },
                        { "Dragon", 1 },
                        { "Electric", 1 },
                        { "Fairy", 2 },
                        { "Fighting", 2 },
                        { "Fire", 1 },
                        { "Flying", 1 },
                        { "Ghost", .5f },
                        { "Grass", 1 },
                        { "Ground", 1 },
                        { "Ice", 1 },
                        { "Normal", 1 },
                        { "Poison", 1 },
                        { "Psychic", 0 },
                        { "Rock", 1 },
                        { "Steel", 1 },
                        { "Water", 1 }
                    }
                },
                new Type(service)
                {
                    Id = TypeEnum.Dragon,
                    Name = "Dragon",
                    DamageTaken = new()
                    {
                        { "(none)", 1 },
                        { "Bug", 1 },
                        { "Dark", 1 },
                        { "Dragon", 2 },
                        { "Electric", .5f },
                        { "Fairy", 2 },
                        { "Fighting", 1 },
                        { "Fire", .5f },
                        { "Flying", 1 },
                        { "Ghost", 1 },
                        { "Grass", .5f },
                        { "Ground", 1 },
                        { "Ice", 2 },
                        { "Normal", 1 },
                        { "Poison", 1 },
                        { "Psychic", 1 },
                        { "Rock", 1 },
                        { "Steel", 1 },
                        { "Water", .5f }
                    }
                },
                new Type(service)
                {
                    Id = TypeEnum.Electric,
                    Name = "Electric",
                    DamageTaken = new()
                    {
                        { "(none)", 1 },
                        { "par", 0 },
                        { "Bug", 1 },
                        { "Dark", 1 },
                        { "Dragon", 1 },
                        { "Electric", .5f },
                        { "Fairy", 1 },
                        { "Fighting", 1 },
                        { "Fire", 1 },
                        { "Flying", .5f },
                        { "Ghost", 1 },
                        { "Grass", 1 },
                        { "Ground", 2 },
                        { "Ice", 1 },
                        { "Normal", 1 },
                        { "Poison", 1 },
                        { "Psychic", 1 },
                        { "Rock", 1 },
                        { "Steel", .5f },
                        { "Water", 1 }
                    }
                },
                new Type(service)
                {
                    Id = TypeEnum.Fairy,
                    Name = "Fairy",
                    DamageTaken = new()
                    {
                        { "(none)", 1 },
                        { "Bug", .5f },
                        { "Dark", .5f },
                        { "Dragon", 0 },
                        { "Electric", 1 },
                        { "Fairy", 1 },
                        { "Fighting", .5f },
                        { "Fire", 1 },
                        { "Flying", 1 },
                        { "Ghost", 1 },
                        { "Grass", 1 },
                        { "Ground", 1 },
                        { "Ice", 1 },
                        { "Normal", 1 },
                        { "Poison", 2 },
                        { "Psychic", 1 },
                        { "Rock", 1 },
                        { "Steel", 2 },
                        { "Water", 1 }
                    }
                },
                new Type(service)
                {
                    Id = TypeEnum.Fighting,
                    Name = "Fighting",
                    DamageTaken = new()
                    {
                        { "(none)", 1 },
                        { "Bug", .5f },
                        { "Dark", .5f },
                        { "Dragon", 1 },
                        { "Electric", 1 },
                        { "Fairy", 2 },
                        { "Fighting", 1 },
                        { "Fire", 1 },
                        { "Flying", 2 },
                        { "Ghost", 1 },
                        { "Grass", 1 },
                        { "Ground", 1 },
                        { "Ice", 1 },
                        { "Normal", 1 },
                        { "Poison", 1 },
                        { "Psychic", 2 },
                        { "Rock", .5f },
                        { "Steel", 1 },
                        { "Water", 1 }
                    }
                },
                new Type(service)
                {
                    Id = TypeEnum.Fire,
                    Name = "Fire",
                    DamageTaken = new()
                    {
                        { "(none)", 1 },
                        { "brn", 0 },
                        { "Bug", .5f },
                        { "Dark", 1 },
                        { "Dragon", 1 },
                        { "Electric", 1 },
                        { "Fairy", .5f },
                        { "Fighting", 1 },
                        { "Fire", .5f },
                        { "Flying", 1 },
                        { "Ghost", 1 },
                        { "Grass", .5f },
                        { "Ground", 2 },
                        { "Ice", .5f },
                        { "Normal", 1 },
                        { "Poison", 1 },
                        { "Psychic", 1 },
                        { "Rock", 2 },
                        { "Steel", .5f },
                        { "Water", 2 }
                    }
                },
                new Type(service)
                {
                    Id = TypeEnum.Flying,
                    Name = "Flying",
                    DamageTaken = new()
                    {
                        { "(none)", 1 },
                        { "Bug", .5f },
                        { "Dark", 1 },
                        { "Dragon", 1 },
                        { "Electric", 2 },
                        { "Fairy", 1 },
                        { "Fighting", .5f },
                        { "Fire", 1 },
                        { "Flying", 1 },
                        { "Ghost", 1 },
                        { "Grass", .5f },
                        { "Ground", 0 },
                        { "Ice", 2 },
                        { "Normal", 1 },
                        { "Poison", 1 },
                        { "Psychic", 1 },
                        { "Rock", 2 },
                        { "Steel", 1 },
                        { "Water", 1 }
                    }
                },
                new Type(service)
                {
                    Id = TypeEnum.Ghost,
                    Name = "Ghost",
                    DamageTaken = new()
                    {
                        { "(none)", 1 },
                        { "trapped", 0 },
                        { "Bug", .5f },
                        { "Dark", 2 },
                        { "Dragon", 1 },
                        { "Electric", 1 },
                        { "Fairy", 1 },
                        { "Fighting", 0 },
                        { "Fire", 1 },
                        { "Flying", 1 },
                        { "Ghost", 2 },
                        { "Grass", 1 },
                        { "Ground", 1 },
                        { "Ice", 1 },
                        { "Normal", 0 },
                        { "Poison", .5f },
                        { "Psychic", 1 },
                        { "Rock", 1 },
                        { "Steel", 1 },
                        { "Water", 1 }
                    }
                },
                new Type(service)
                {
                    Id = TypeEnum.Grass,
                    Name = "Grass",
                    DamageTaken = new()
                    {
                        { "(none)", 1 },
                        { "powder", 0 },
                        { "Bug", 2 },
                        { "Dark", 1 },
                        { "Dragon", 1 },
                        { "Electric", .5f },
                        { "Fairy", 1 },
                        { "Fighting", 1 },
                        { "Fire", 2 },
                        { "Flying", 2 },
                        { "Ghost", 1 },
                        { "Grass", .5f },
                        { "Ground", .5f },
                        { "Ice", 2 },
                        { "Normal", 1 },
                        { "Poison", 2 },
                        { "Psychic", 1 },
                        { "Rock", 1 },
                        { "Steel", 1 },
                        { "Water", .5f }
                    }
                },
                new Type(service)
                {
                    Id = TypeEnum.Ground,
                    Name = "Ground",
                    DamageTaken = new()
                    {
                        { "(none)", 1 },
                        { "sandstorm", 0 },
                        { "Bug", 1 },
                        { "Dark", 1 },
                        { "Dragon", 1 },
                        { "Electric", 0 },
                        { "Fairy", 1 },
                        { "Fighting", 1 },
                        { "Fire", 1 },
                        { "Flying", 1 },
                        { "Ghost", 1 },
                        { "Grass", 2 },
                        { "Ground", 1 },
                        { "Ice", 2 },
                        { "Normal", 1 },
                        { "Poison", .5f },
                        { "Psychic", 1 },
                        { "Rock", .5f },
                        { "Steel", 1 },
                        { "Water", 2 }
                    }
                },
                new Type(service)
                {
                    Id = TypeEnum.Ice,
                    Name = "Ice",
                    DamageTaken = new()
                    {
                        { "(none)", 1 },
                        { "hail", 0 },
                        { "frz", 0 },
                        { "Bug", 1 },
                        { "Dark", 1 },
                        { "Dragon", 1 },
                        { "Electric", 1 },
                        { "Fairy", 1 },
                        { "Fighting", 2 },
                        { "Fire", 2 },
                        { "Flying", 1 },
                        { "Ghost", 1 },
                        { "Grass", 1 },
                        { "Ground", 1 },
                        { "Ice", .5f },
                        { "Normal", 1 },
                        { "Poison", 1 },
                        { "Psychic", 1 },
                        { "Rock", 2 },
                        { "Steel", 2 },
                        { "Water", 1 }
                    }
                },
                new Type(service)
                {
                    Id = TypeEnum.Normal,
                    Name = "Normal",
                    DamageTaken = new()
                    {
                        { "(none)", 1 },
                        { "Bug", 1 },
                        { "Dark", 1 },
                        { "Dragon", 1 },
                        { "Electric", 1 },
                        { "Fairy", 1 },
                        { "Fighting", 2 },
                        { "Fire", 1 },
                        { "Flying", 1 },
                        { "Ghost", 0 },
                        { "Grass", 1 },
                        { "Ground", 1 },
                        { "Ice", 1 },
                        { "Normal", 1 },
                        { "Poison", 1 },
                        { "Psychic", 1 },
                        { "Rock", 1 },
                        { "Steel", 1 },
                        { "Water", 1 }
                    }
                },
                new Type(service)
                {
                    Id = TypeEnum.Poison,
                    Name = "Poison",
                    DamageTaken = new()
                    {
                        { "(none)", 1 },
                        { "psn", 0 },
                        { "tox", 0 },
                        { "Bug", .5f },
                        { "Dark", 1 },
                        { "Dragon", 1 },
                        { "Electric", 1 },
                        { "Fairy", .5f },
                        { "Fighting", .5f },
                        { "Fire", 1 },
                        { "Flying", 1 },
                        { "Ghost", 1 },
                        { "Grass", .5f },
                        { "Ground", 2 },
                        { "Ice", 1 },
                        { "Normal", 1 },
                        { "Poison", .5f },
                        { "Psychic", 2 },
                        { "Rock", 1 },
                        { "Steel", 1 },
                        { "Water", 1 }
                    }
                },
                new Type(service)
                {
                    Id = TypeEnum.Psychic,
                    Name = "Psychic",
                    DamageTaken = new()
                    {
                        { "(none)", 1 },
                        { "Bug", 2 },
                        { "Dark", 2 },
                        { "Dragon", 1 },
                        { "Electric", 1 },
                        { "Fairy", 1 },
                        { "Fighting", .5f },
                        { "Fire", 1 },
                        { "Flying", 1 },
                        { "Ghost", 2 },
                        { "Grass", 1 },
                        { "Ground", 1 },
                        { "Ice", 1 },
                        { "Normal", 1 },
                        { "Poison", 1 },
                        { "Psychic", .5f },
                        { "Rock", 1 },
                        { "Steel", 1 },
                        { "Water", 1 }
                    }
                },
                new Type(service)
                {
                    Id = TypeEnum.Rock,
                    Name = "Rock",
                    DamageTaken = new()
                    {
                        { "(none)", 1 },
                        { "sandstorm", 0 },
                        { "Bug", 1 },
                        { "Dark", 1 },
                        { "Dragon", 1 },
                        { "Electric", 1 },
                        { "Fairy", 1 },
                        { "Fighting", 2 },
                        { "Fire", .5f },
                        { "Flying", .5f },
                        { "Ghost", 1 },
                        { "Grass", 2 },
                        { "Ground", 2 },
                        { "Ice", 1 },
                        { "Normal", .5f },
                        { "Poison", .5f },
                        { "Psychic", 1 },
                        { "Rock", 1 },
                        { "Steel", 2 },
                        { "Water", 2 }
                    }
                },
                new Type(service)
                {
                    Id = TypeEnum.Steel,
                    Name = "Steel",
                    DamageTaken = new()
                    {
                        { "(none)", 1 },
                        { "psn", 0 },
                        { "tox", 0 },
                        { "sandstorm", 0 },
                        { "Bug", .5f },
                        { "Dark", 1 },
                        { "Dragon", .5f },
                        { "Electric", 1 },
                        { "Fairy", .5f },
                        { "Fighting", 2 },
                        { "Fire", 2 },
                        { "Flying", .5f },
                        { "Ghost", 1 },
                        { "Grass", .5f },
                        { "Ground", 2 },
                        { "Ice", .5f },
                        { "Normal", .5f },
                        { "Poison", 0 },
                        { "Psychic", .5f },
                        { "Rock", .5f },
                        { "Steel", .5f },
                        { "Water", 1 }
                    }
                },
                new Type(service)
                {
                    Id = TypeEnum.Water,
                    Name = "Water",
                    DamageTaken = new()
                    {
                        { "(none)", 1 },
                        { "Bug", 1 },
                        { "Dark", 1 },
                        { "Dragon", 1 },
                        { "Electric", 2 },
                        { "Fairy", 1 },
                        { "Fighting", 1 },
                        { "Fire", .5f },
                        { "Flying", 1 },
                        { "Ghost", 1 },
                        { "Grass", 2 },
                        { "Ground", 1 },
                        { "Ice", .5f },
                        { "Normal", 1 },
                        { "Poison", 1 },
                        { "Psychic", 1 },
                        { "Rock", 1 },
                        { "Steel", .5f },
                        { "Water", .5f }
                    }
                }
            };
            return typeChart;
        }
    }
}
