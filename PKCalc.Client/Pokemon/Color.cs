using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKCalc.Client.Pokemon
{
    public class Color
    {
        private readonly PokemonService _service;
        
        public ColorEnum Id { get; private set; }
        public string Name { get; private set; }

        private Color(PokemonService service)
        {
            this._service = service;
        }

        public static IEnumerable<Color> GetColors(PokemonService service)
        {
            PokemonService.Instance.Logger.Debug("Getting colors...");
            return new List<Color>
            {
                new Color(service) { Id = ColorEnum.None, Name = "(none)" },
                new Color(service) { Id = ColorEnum.Green, Name = "Green" },
                new Color(service) { Id = ColorEnum.Red, Name = "Red" },
                new Color(service) { Id = ColorEnum.Blue, Name = "Blue" },
                new Color(service) { Id = ColorEnum.Yellow, Name = "Yellow" },
                new Color(service) { Id = ColorEnum.Black, Name = "Black" },
                new Color(service) { Id = ColorEnum.White, Name = "White" },
                new Color(service) { Id = ColorEnum.Brown, Name = "Brown" },
                new Color(service) { Id = ColorEnum.Purple, Name = "Purple" },
                new Color(service) { Id = ColorEnum.Gray, Name = "Gray" },
                new Color(service) { Id = ColorEnum.Pink, Name = "Pink" },
            };
        }
    }
}
