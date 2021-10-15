using System;
using System.Net;
using System.Diagnostics;
using System.ComponentModel;

namespace Stationeers.Addons.Updater
{
    class UpdaterType : TypeUpdate
    {
        private static int GameId = 544550;

        public UpdaterType(string LocalFileVersionUrl, string RemoteFileVersionUrl, string LocalFileUrl, string RemoteFileUrl)
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
            Console.WriteLine("Downloaded Updater " + int.Parse(Math.Truncate(percentage).ToString()) + "%");
        }

        public override void DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            DeleteLocalFilesFromXml();
        }

        public override void StartGame()
        {
            Process.Start("CMD.exe", "/C start steam://rungameid/" + GameId);
        }

    }
}
