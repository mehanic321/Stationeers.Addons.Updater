using System;
using System.Net;
using System.Diagnostics;
using System.ComponentModel;
using System.IO.Compression;

namespace Stationeers.Addons.Modules.Updater
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
        public override void StartUpdater()
        {
            Process.Start("updater/Stationeers.Addons.Updater.exe");
        }

        public override void CheckNeedUpdate()
        {
            CheckError();

            if (GetVersion(LocalFileVersion).CompareTo(GetVersion(RemoteFileVersion)) != 0)
            {
                StartUpdater();
                Process.GetCurrentProcess().Kill();
                theend = true;
            }
            else
            {
                theend = true;
            }
        }

    }
}
