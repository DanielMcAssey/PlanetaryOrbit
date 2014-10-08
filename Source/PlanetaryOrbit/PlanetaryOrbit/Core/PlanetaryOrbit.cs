using System;
using POSystem;
using PlanetaryOrbit.Core;

namespace PlanetaryOrbit
{
#if WINDOWS || XBOX
    static class PlanetaryOrbit
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
#if RELEASE
            try
            {
#endif
                Logger.init();

                using (Game game = new Game())
                {
                    game.Run();
                }
#if RELEASE
            }
            catch (Exception ex)
            {
                Logger.log(Log_Type.ERROR, "FATAL: " + ex.Message);
            }
#endif
        }
    }
#endif
}

