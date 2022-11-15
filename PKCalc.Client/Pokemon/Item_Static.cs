using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKCalc.Client.Pokemon
{
    public partial class Item
    {
        public static IEnumerable<Item> GetItems(PokemonService service)
        {
            return Enum.GetValues(typeof(ItemEnum)).Cast<ItemEnum>().Select(i => new Item(service) { Id = i });
        }
    }
}
