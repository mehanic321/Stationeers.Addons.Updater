using System;
using System.Net;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Compression;

namespace Stationeers.Addons.Modules.Updater
{
    class MainType : TypeUpdate
    {
        public MainType(string LocalFileVersionUrl, string RemoteFileVersionUrl, string LocalFileUrl, string RemoteFileUrl)
        {
            this.LocalFileVersionUrl = LocalFileVersionUrl;
            this.RemoteFileVersionUrl = RemoteFileVersionUrl;
            this.LocalFileUrl = LocalFileUrl;
            this.RemoteFileUrl = RemoteFileUrl;
            DownloadVersionFiles();
        }

        public override void DownloadFileProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;
            Console.WriteLine("Downloaded Main " + int.Parse(Math.Truncate(percentage).ToString()) + "%");
        }

        public override void DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            DeleteLocalFilesFromXml();
        }

        public override void ExtractFiles()
        {
            if (ExtractToDirectory(ZipFile.OpenRead(LocalFileUrl), "./", true))
            {
                DeleteIo(LocalFileVersionUrl, true);
                SetVersion(ref LocalFileVersion, GetVersion(RemoteFileVersion).ToString());
                LocalFileVersion.Save(LocalFileVersionUrl);
                theend = true;
            }
            else
            {
                theend = true;
            }
        }

        public override void CheckNeedUpdate()
        {
            CheckError();

            if (GetVersion(LocalFileVersion).CompareTo(GetVersion(RemoteFileVersion)) != 0)
            {
                DownloadFile();
            }
            else
            {
                theend = true;
            }
        }

    }
}
