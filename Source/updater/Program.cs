using System;
using System.Diagnostics;
using Stationeers.Addons.Updater;

namespace updater
{

    static class Program
    {
        private static int gameId = 544550;

        private static bool isStart = false;
        private static bool isError = false;

        private static UpdaterType UpdaterType = new UpdaterType(        
            "version.xml",
            "https://mehanic321.github.io/stationeersaddonsupdaterfiles.github.io/files/version.xml",
            "main.zip",
            "https://mehanic321.github.io/stationeersaddonsupdaterfiles.github.io/files/main.zip"
        );

        /*private static MainType MainType = new MainType(        
            "versionupdater.xml",
            "https://mehanic321.github.io/stationeersaddonsupdaterfiles.github.io/files/versionupdater.xml",
            "updater.zip",
            "https://mehanic321.github.io/stationeersaddonsupdaterfiles.github.io/files/updater.zip"
        );*/

        static void Main(string[] args)
        {
            while (!UpdaterType.theend) {               
                var runningProcs = Process.GetProcessesByName("rocketstation");
                if (runningProcs.Length <= 0 && !isStart)
                {
                    isStart = true;
                }
                else
                {
                    if (!isError)
                    {
                        isError = true;
                        Console.WriteLine("Wait for the game to close or close the game yourself ...");
                    }                        
                }                
            }
        }


    }
}
