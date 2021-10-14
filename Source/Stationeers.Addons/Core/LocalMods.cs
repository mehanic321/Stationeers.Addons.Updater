﻿// Stationeers.Addons (c) 2018-2021 Damian 'Erdroy' Korczowski & Contributors

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

// TODO: We probably should refactor all of that.

namespace Stationeers.Addons.Core
{
    internal static class LocalMods
    {
        private const string DebugPluginPostfix = "-Debug";
        
        // Select mods directory based on install instance
        private static readonly string LocalModsDirectory = LoaderManager.IsDedicatedServer
            ? GetDedicatedServerModsDirectory()
            : Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/My Games/Stationeers/mods/";

        public static IEnumerable<string> GetLocalModDirectories(bool includeDebugPlugins = true, bool skipIfDebugPluginExists = false)
        {
            if ((LocalModsDirectory == null) || !Directory.Exists(LocalModsDirectory))
            {
                Debug.Log("ModLoader ERROR: Could not locate mods directory, no mods getting initialized.");
                return new string[] { };
            }

            Debug.Log($"Trying to load mod from {LocalModsDirectory}");

            var directories = Directory.GetDirectories(LocalModsDirectory);
            var modDirectory = new List<string>();

            foreach (var directory in directories)
            {
                if (!includeDebugPlugins && directory.EndsWith(DebugPluginPostfix)) continue;
                
                // Skip if this is not debug plugin and there is debug version of it
                // Messy, but works for now
                if (!directory.EndsWith(DebugPluginPostfix) && skipIfDebugPluginExists &&
                    directories.Any(x => x != directory && x.Contains(directory))) continue;
                
                modDirectory.Add(directory);
            }

            return modDirectory.ToArray();
        }

        private static string GetDedicatedServerModsDirectory()
        {
            // Find serverside default.ini
            // Currently needs to be in the root of the dedicated server directory (next to rocketstation_DedicatedServer.exe)
            if (!File.Exists("default.ini"))
                Debug.LogWarning("default.ini file not found!");
            
            foreach(var line in File.ReadLines("default.ini"))
            {
                if (!line.Contains("MODPATH=")) continue;
                
                Debug.Log($"Found mod path: {line.Substring(8)}");
                return line.Substring(8);
            }
            
            return null;
        }

        public static IEnumerable<string> GetLocalModDebugAssemblies()
        {
            if (!Directory.Exists(LocalModsDirectory)) return new string[] { };
            
            var directories = GetLocalModDirectories();
            var modAssemblies = new List<string>();

            foreach (var directory in directories)
            {
                if (!directory.EndsWith(DebugPluginPostfix)) continue;
                var pluginName = Directory.GetParent(directory + "\\")?.Name + ".dll";
                var pluginDebugAssembly = Path.Combine(directory, pluginName);

                if (File.Exists(pluginDebugAssembly))
                    modAssemblies.Add(pluginDebugAssembly);
            }

            return modAssemblies.ToArray();
        }
    }
}
