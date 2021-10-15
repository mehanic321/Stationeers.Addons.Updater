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
    class UpdateManager : MonoBehaviour
    {
        private MainType MainType = new MainType(
            "versionupdater.xml",
            "https://mehanic321.github.io/stationeersaddonsupdaterfiles.github.io/files/versionupdater.xml",
            "updater.zip",
            "https://mehanic321.github.io/stationeersaddonsupdaterfiles.github.io/files/updater.zip"
        );

        private UpdaterType UpdaterType = null;

        void Update()
        {
            if (MainType.theend)
            {

                if (UpdaterType == null)
                {
                    UpdaterType = new UpdaterType(
                        "version.xml",
                        "https://mehanic321.github.io/stationeersaddonsupdaterfiles.github.io/files/version.xml",
                        "",
                        ""
                    );
                }
            }
        }

    }
}
