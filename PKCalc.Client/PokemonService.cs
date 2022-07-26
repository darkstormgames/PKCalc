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
        public static bool IsOnline => NetworkHelper.IsOnline;


        public Logger.AppLog Logger { get; private set; }

        private PokemonService()
        {
            PokemonService.instance = this;

#if DEBUG
            this.Logger = new Logger.AppLog(
                minLogLevel: NLog.LogLevel.Trace,
                useMetrics: true,
                debugLogging: true);
#else
            
#endif

        }
    }
}