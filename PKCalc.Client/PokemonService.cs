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
        public ObservableCollection<Logger.InternalLogItem> InternalLog { get; } = new();

        private PokemonService()
        {
            
        }
    }
}