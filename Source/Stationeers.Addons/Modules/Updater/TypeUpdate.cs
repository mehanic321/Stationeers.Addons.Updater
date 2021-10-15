using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Xml;
using System.IO;
using System.IO.Compression;

namespace Stationeers.Addons.Modules.Updater
{
    class TypeUpdate
    {
        public string LocalFileVersionUrl;
        public string RemoteFileVersionUrl;

        public string LocalFileUrl;
        public string RemoteFileUrl;

        public XmlDocument LocalFileVersion = new XmlDocument();
        public XmlDocument RemoteFileVersion = new XmlDocument();

        public List<string> Errors = new List<string>();

        public bool theend = false;

        public virtual void DownloadFileProgress(object sender, DownloadProgressChangedEventArgs e) { }
        public virtual void DownloadFileCompleted(object sender, AsyncCompletedEventArgs e) { }
        public virtual void StartGame() { }
        public virtual void StartUpdater() { }
        public virtual void CheckNeedUpdate() { }
        public virtual void ExtractFiles() { }

        public void DownloadVersionFiles()
        {
            try
            {
                LocalFileVersion.Load(LocalFileVersionUrl);
                RemoteFileVersion.Load(RemoteFileVersionUrl);
            }
            catch (Exception ex)
            {
                AddError(ex.Message);
            }

            CheckNeedUpdate();
        }

        public void DownloadFile()
        {
            WebClient webClient = new WebClient();
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadFileProgress);
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCompleted);
            webClient.DownloadFileAsync(new Uri(RemoteFileUrl), LocalFileUrl);
        }

        public Version GetVersion(XmlDocument file)
        {
            return new Version(file.SelectSingleNode("StationeersAddons/Version").LastChild.InnerText);
        }

        public void SetVersion(ref XmlDocument file, string version)
        {
            file.SelectSingleNode("StationeersAddons/Version").LastChild.InnerText = version;
        }

        public void SaveXml()
        {
            CheckError();
        }

        public void DeleteLocalFilesFromXml()
        {
            XmlNodeList xnList = LocalFileVersion.SelectNodes("StationeersAddons/Delete/Dir");
            foreach (XmlNode xn in xnList)
            {
                foreach (XmlElement xn2 in xn.ChildNodes)
                {
                    DeleteIo(xn2.InnerText);
                }
            }

            xnList = LocalFileVersion.SelectNodes("StationeersAddons/Delete/File");
            foreach (XmlNode xn in xnList)
            {
                DeleteIo(xn["Item"].InnerText, true);
            }

            ExtractFiles();
        }

        public bool ExtractToDirectory(ZipArchive archive, string destinationDirectoryName, bool overwrite)
        {
            if (!overwrite)
            {
                archive.ExtractToDirectory(destinationDirectoryName);
                return true;
            }
            foreach (ZipArchiveEntry file in archive.Entries)
            {
                string completeFileName = Path.Combine(destinationDirectoryName, file.FullName);
                if (file.Name == "")
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(completeFileName));
                    continue;
                }
                file.ExtractToFile(completeFileName, true);
            }
            return true;
        }

        public static void DeleteIo(string path, bool isFile = false)
        {
            if (isFile)
            {
                if (File.Exists(path)) File.Delete(path);
            }
            else
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
            }
        }

        public void CheckError()
        {
            if (Errors.Count > 0)
            {
                foreach (string Error in Errors)
                {
                    Console.WriteLine(Error);
                }
                Environment.Exit(0);
            }
        }

        public void AddError(string error)
        {
            Errors.Add(error);
        }
    }
}
