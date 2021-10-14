using System;
using System.ComponentModel;
using System.Net;
using System.Xml;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using UnityEngine;

namespace Stationeers.Addons.Modules.Updater
{
    class UpdateManager
    {

        private static XmlDocument VersionFileOld;
        private static XmlDocument VersionFileNew;

        private static Version VersionOld;
        private static Version VersionNew;

        private static string NameDownloadFileUpdater = "update.zip";
        //private static string UrlDownloadFileUpdater = "http://test.test/" + NameDownloadFileUpdater;
        private static string UrlDownloadFileUpdater = "https://drive.google.com/uc?export=download&confirm=no_antivirus&id=1k7kMVZXEdsyvZqVq1-YWdr3epceZ5a8Z";

        private static string NameVersionFile = "version.xml";
        //private static string UrlVersionFile = "http://test.test/" + NameVersionFile;
        private static string UrlVersionFile = "https://drive.google.com/uc?export=download&confirm=no_antivirus&id=1ef6LJEWk1Rj4IlQUZDXM0OE8BIqjT4cq";

        public static bool theend = false;
        public static bool isRestart = false;

        /*
         ...проверить если update.exe старый то обновить его и записать новую версию в version.xml
        
        если новый то проверить версию compilemod.exe
        если версия compilemod.exe старая то запустить update.exe
        закрыть compilemod.exe
         */

        public static void StartUpdate()
        {            
            DownloadUpdater();
        }

        private static void DownloadUpdater()
        {

            UnityEngine.Debug.Log("Download version.xml");
            VersionFileNew = Updater.GetXmlFile(UrlVersionFile);
            UnityEngine.Debug.Log("Download version.xml complete");
            VersionNew = new Version(Updater.GetXmlNodeText(VersionFileNew, "/StationeersAddons/Updater", "Version"));

            if (File.Exists(NameVersionFile))
            {
                VersionFileOld = Updater.GetXmlFile(NameVersionFile);
                VersionOld = new Version(Updater.GetXmlNodeText(VersionFileOld, "/StationeersAddons/Updater", "Version"));
            }
            else
            {
                VersionFileOld = VersionFileNew;
                VersionOld = new Version("0.0.0.0");
            }

            StartDownloadIfVersionOld(UrlDownloadFileUpdater, NameDownloadFileUpdater, VersionNew, VersionOld);
        }

        public static void StartDownloadIfVersionOld(string ZipFileUrl, string OutFile, Version NewVersion, Version OldVersion)
        {
            WebClient webClient = new WebClient();

            if (NewVersion.CompareTo(OldVersion) > 0)
            {
                UnityEngine.Debug.Log("Start Updater download");
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressChangedUpdater);
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(CompletedDownloadUpdater);
                webClient.DownloadFileAsync(new Uri(ZipFileUrl), OutFile);
            }
            else
            {
                UnityEngine.Debug.Log("No update required");
                theend = true;
            }
        }

        public static void DownloadProgressChangedUpdater(object sender, DownloadProgressChangedEventArgs e)
        {
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;
            UnityEngine.Debug.Log("Downloaded Updater " + int.Parse(Math.Truncate(percentage).ToString()) + "%");
        }

        public static void CompletedDownloadUpdater(object sender, AsyncCompletedEventArgs e)
        {
            UnityEngine.Debug.Log("Download Updater complete");            

            UnityEngine.Debug.Log("Deletion Main directories and files");
            Updater.DeleteIoByXml(VersionFileNew, "/StationeersAddons/Updater");
            UnityEngine.Debug.Log("Deletion Main directories and files complete");

            UnityEngine.Debug.Log("Start Updater unpacking, wait...");

            if (Updater.ExtractToDirectory(ZipFile.OpenRead(NameDownloadFileUpdater), "./", true))
            {
                UnityEngine.Debug.Log("Unpacking complete");

                UnityEngine.Debug.Log("Deletion version.xml");
                Updater.DeleteIo("version.xml", true);
                UnityEngine.Debug.Log("Deletion version.xml complete");

                XmlNode xmlNode = VersionFileOld.SelectSingleNode("/StationeersAddons/Updater/Version");
                xmlNode.LastChild.InnerText = VersionNew.ToString();

                UnityEngine.Debug.Log("Save version.xml");
                VersionFileOld.Save("version.xml");
                UnityEngine.Debug.Log("Save version.xml complete");


                UnityEngine.Debug.Log("Download version.xml");
                VersionFileNew = Updater.GetXmlFile(UrlVersionFile);
                UnityEngine.Debug.Log("Download version.xml complete");
                VersionNew = new Version(Updater.GetXmlNodeText(VersionFileNew, "/StationeersAddons/Main", "Version"));

                if (File.Exists(NameVersionFile))
                {
                    VersionFileOld = Updater.GetXmlFile(NameVersionFile);
                    VersionOld = new Version(Updater.GetXmlNodeText(VersionFileOld, "/StationeersAddons/Main", "Version"));
                }
                else
                {
                    VersionFileOld = VersionFileNew;
                    VersionOld = new Version("0.0.0.0");
                }

                if (VersionNew.CompareTo(VersionOld) > 0)
                {
                    UnityEngine.Debug.Log("Start updater...");
                    UnityEngine.Debug.Log("CURRENT DIR "+Directory.GetCurrentDirectory());
                    Process.Start("updater/Stationeers.Addons.Updater.exe");
                    isRestart = true;
                }
                else
                {
                    theend = true;
                }
            }
            else
            {
                UnityEngine.Debug.Log("Unpacking failed, exit");
            }
            theend = true;
        }
    }
}
