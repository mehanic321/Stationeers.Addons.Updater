using System;
using System.ComponentModel;
using System.Net;
using System.Xml;
using System.IO;
using System.IO.Compression;
using Stationeers.Addons.Updater;

namespace Stationeers.Addons.Updater
{
    static class Updater
    {
        public static XmlDocument GetXmlFile(string href)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(href);
            return xDoc;
        }

        public static string GetXmlNodeText(XmlDocument xml, string path, string node)
        {
            XmlNodeList xnList = xml.SelectNodes(path);
            foreach (XmlNode xn in xnList)
            {
                return xn[node].InnerText;
            }
            return "";
        }

        public static bool ExtractToDirectory(this ZipArchive archive, string destinationDirectoryName, bool overwrite)
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

        public static bool DeleteIoByXml(XmlDocument xml, string MainPath)
        {
            XmlNodeList xnList = xml.SelectNodes(MainPath + "/Delete/Dir");
            foreach (XmlNode xn in xnList)
            {
                foreach (XmlElement xn2 in xn.ChildNodes)
                {
                    DeleteIo(xn2.InnerText);
                }
            }

            xnList = xml.SelectNodes(MainPath + "/Delete/File");
            foreach (XmlNode xn in xnList)
            {
                DeleteIo(xn["Item"].InnerText, true);
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
                    Directory.Delete(path);
                }
            }
        }

        
    }
}
