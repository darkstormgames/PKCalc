using PKCalc.Net;

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
                    instance = new();
                return instance;
            }
        }

        /// <summary>
        /// Checks, if the PC is connected to the internet.
        /// </summary>
        public bool IsOnline => NetworkHelper.IsOnline;

        private Logger.AppLog logger;
        public Logger.AppLog Logger
        {
            get
            {
                return logger;
            }
            set
            {
                if (logger == null)
                    logger = value;
            }
        }

        private PokemonService()
        {
            
        }
    }
}