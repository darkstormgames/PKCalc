using PKCalc.Net;
using System.Collections.ObjectModel;

namespace PKCalc.Client
{
    public class PokemonService
    {
        // using a singleton pattern to make sure only one instance can exist at all times
        private static PokemonService instance;
        public static PokemonService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new();
                    instance.Logger.Trace("Creating new instance of PokemonService.");
                }
                return instance;
            }
        }

        /// <summary>
        /// Checks, if the PC is connected to the internet.
        /// </summary>
        public bool IsOnline => NetworkHelper.IsOnline;

        private Logger.Log logger;
        public Logger.Log Logger
        {
            get
            {
                if (logger == null)
                {
                    logger = new(minLogLevel: Configuration.RegistryHelper.GetLogLevel(),
                                 internalLogItems: InternalLog);
                }
                return logger;
            }
        }
        public ObservableCollection<Logger.InternalLogItem> InternalLog { get; }

        public IEnumerable<Pokemon.Ability> Abilities { get; private set; }
        public IEnumerable<Pokemon.Color> Colors { get; private set; }
        public IEnumerable<Pokemon.EggGroup> EggGroups { get; private set; }
        public IEnumerable<Pokemon.Nature> Natures { get; private set; }
        public IEnumerable<Pokemon.Type> Types { get; private set; }

        private PokemonService()
        {
            this.InternalLog = new();
        }

        public void ReloadCache()
        {
            this.Logger.Trace("Reloading cache.");
            this.Abilities = Pokemon.Ability.GetAbilities(this).OrderBy(o => o.Name);
            this.Colors = Pokemon.Color.GetColors(this);
            this.EggGroups = Pokemon.EggGroup.GetEggGroups(this);
            this.Natures = Pokemon.Nature.GetNatures(this);
            this.Types = Pokemon.Type.GetTypeChart(this);

        }
    }
}