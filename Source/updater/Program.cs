using System;
using System.ComponentModel;
using System.Net;
using System.Xml;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using Stationeers.Addons.Updater;

namespace updater
{
    static class Program
    {
        private static int gameId = 544550;

        private static XmlDocument VersionFileOld;       
        private static XmlDocument VersionFileNew;

        private static Version VersionOld;
        private static Version VersionNew;

        private static string NameDownloadFileMain = "main.zip";
        private static string UrlDownloadFileMain = "http://test.test/" + NameDownloadFileMain;

        private static string NameVersionFile = "version.xml";
        private static string UrlVersionFile = "http://test.test/" + NameVersionFile;

        private static bool theend = false;
        private static bool isStart = false;

        /*
        запуск игры

        проверить если update.exe старый то обновить его и записать новую версию в version.xml
        если новый то проверить версию compilemod.exe
        если версия compilemod.exe старая то запустить update.exe
        закрыть compilemod.exe

        2 часть готова
        +update.exe скачивает новую версию compilemod.exe, удаляет старое, распаковывает
        +запускает compilemod.exe
        +закрывается сам
        */

        static void Main(string[] args)
        {
            while (!theend) {
                if (!theend) {
                    var runningProcs = Process.GetProcessesByName("rocketstation");
                    if (runningProcs.Length <= 0 && !isStart)
                    {
                        DownloadMain();
                        isStart = true;
                    }
                    else
                    {
                        Console.WriteLine("Wait for the game to close or close the game yourself ...");
                    }
                }
            }
        }

        private static void DownloadMain()
        {

            Console.WriteLine("Download version.xml");
            VersionFileNew = Updater.GetXmlFile(UrlVersionFile);
            Console.WriteLine("Download version.xml complete");
            VersionNew = new Version(Updater.GetXmlNodeText(VersionFileNew, "/StationeersAddons/Main", "Version"));

            if (File.Exists("../" + NameVersionFile))
            {
                VersionFileOld = Updater.GetXmlFile("../" + NameVersionFile);
                VersionOld = new Version(Updater.GetXmlNodeText(VersionFileOld, "/StationeersAddons/Main", "Version"));
            }
            else
            {
                VersionFileOld = VersionFileNew;
                VersionOld = new Version("0.0.0.0");                
            }

            StartDownloadIfVersionOld(UrlDownloadFileMain, NameDownloadFileMain, VersionNew, VersionOld);
        }

        public static void StartDownloadIfVersionOld(string ZipFileUrl, string OutFile, Version NewVersion, Version OldVersion)
        {
            WebClient webClient = new WebClient();

            if (NewVersion.CompareTo(OldVersion) > 0)
            {
                Console.WriteLine("Start Main download");
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressChangedMain);
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(CompletedDownloadMain);
                webClient.DownloadFileAsync(new Uri(ZipFileUrl), "../" + OutFile);
            }
            else
            {
                Console.WriteLine("No update required");
                theend = true;
            }
        }

        public static void DownloadProgressChangedMain(object sender, DownloadProgressChangedEventArgs e)
        {
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;
            Console.WriteLine("Downloaded Main " + int.Parse(Math.Truncate(percentage).ToString()) + "%");
        }

        public static void CompletedDownloadMain(object sender, AsyncCompletedEventArgs e)
        {
            Console.WriteLine("Download Main complete");

            Console.WriteLine("Deletion Main directories and files");
            Updater.DeleteIoByXml(VersionFileNew, "/StationeersAddons/Main");
            Console.WriteLine("Deletion Main directories and files complete");

            Console.WriteLine("Start Main unpacking, wait...");

            if (Updater.ExtractToDirectory(ZipFile.OpenRead("../" + NameDownloadFileMain), "../", true))
            {
                Console.WriteLine("Unpacking complete");

                Console.WriteLine("Deletion version.xml");
                Updater.DeleteIo("../version.xml", true);
                Console.WriteLine("Deletion version.xml complete");

                XmlNode xmlNode = VersionFileOld.SelectSingleNode("/StationeersAddons/Main/Version");
                xmlNode.LastChild.InnerText = VersionNew.ToString();

                Console.WriteLine("Save version.xml");
                VersionFileOld.Save("../version.xml");
                Console.WriteLine("Save version.xml complete");

                Console.WriteLine("Start game...");
                Process.Start("CMD.exe", "/C start steam://rungameid/" + gameId);
            }
            else
            {
                Console.WriteLine("Unpacking failed, exit");
            }
            theend = true;
        }

    }
}
