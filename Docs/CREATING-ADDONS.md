# Stationeers.Addons

## Basic addon with simple plugin.
Our addons work this standard mods. Besides the `About` and `GameData` folders, we are proving our additional two folders that you can use to mod the game: `Scripts` and `Content`. Scripts folder is being used to hold all of the custom code and Content is designed to hold custom content i.e. asset bundles (more about that [here](CUSTOM-CONTENT.md)).

To get basic addon with simple plugin, follow these steps:
1. Make sure that you have Visual Studio 2019 installed with C# support. You can also install `Visual Studio Tools for Unity`, this will come in handy later, when you will want to debug your addon. Additionally it will be helpful, if you know how to make standard mods - [read this](https://stationeers-wiki.com/Guide_(Modding)).
2. Clone [Stationeers.Addons](https://github.com/Erdroy/Stationeers.Addons) repository using git or download it using Download button, open the resulting folder.
3. Copy dll files from your game installation directory into Libraries/Stationeers, [read this](https://github.com/Erdroy/Stationeers.Addons/blob/master/Libraries/Stationeers/README.md).
4. Copy ExampleMod from `Source/ExampleMods` into `Source/MyMods/` directory.
5. Rename copied directory to whatever name you want, let's keep it simple, and call it MyFirstAddon.
6. Rename `ExampleMod.csproj` to `MyFirstAddon.csproj` and  `ExampleMod.csproj.user` to `MyFirstAddon.csproj.user`.
7. Open `MyFirstAddon.csproj` file in your favorite text editor (not IDE!).
8. Change these two lines:
   ```xml
   <RootNamespace>ExampleMod</RootNamespace>
   <AssemblyName>ExampleMod-Debug</AssemblyName>
   ```
   to
   ```xml
   <RootNamespace>MyFirstAddon</RootNamespace>
   <AssemblyName>MyFirstAddon-Debug</AssemblyName>
   ```
9. Go back to the Source folder and open `Stationeers.Addons.sln` solution file.
10. Click *RMB* on *"My Mods"* folder in solution, select `Add > Existing Project...`, locate your addon C# project (*MyFirstAddon.csproj*).
11. Rename `ExampleMod.cs` to `MyFirstAddon.cs`, as this will be your entry point. You should also change the namespace to.
12. To check if your addon is working, go to `C:\Users\%username%\Documents\My Games\Stationeers\` and open `mods` folder (you can safely create it, if it's missing). Create folder named like your Addon and copy `About`, `Content`, `GameData` and `Scripts`. And that's it.
13. At this point, you have the ability to start writing the code. Check MyFirstAddon.cs for ideas. Have fun.
14. Useful links:
    * [Debugging addons](DEBUGGING-ADDONS.md)
    * [Creating custom content](CUSTOM-CONTENT.md) (Coming soon)
    * [Overloading methods](OVERLOADING-METHODS.md) (Coming soon)
    * [Harmony documentation](https://harmony.pardeike.net/articles/intro.html) - Use it for patching the game's code at runtime.
    * [dnSpy](https://github.com/dnSpy/dnSpy) - for browsing the game's source code.

## Addon Development

By default, addons are only compiled once at start up. Starting the game with the `--live-reload` option will enable a new shortcut, `Ctrl+R`, to recompile and reload addons. You can add the option in the game's properties in Steam.

With live reload enabled, be mindful of the plugin lifecycle - not disposing a resource created by an addon in its `Unload` method might lead to unexpected behaviours.

## Publishing your addon.
We are supporting workshop! Yes! It's as simple as publishing your mod (created in step **11** above) to the workshop!

But please, make sure that people know that it's an addon and not a standard mod by following these steps:
1. If you have custom thumbnail - include the addon [overlay](../Source/AddonOverlay.png).
2. Add this text to the workshop description at the beginning:
```
WARNING: This mod will not work, unless you install Stationeers.Addon mod loader.
Read how to install it here: https://github.com/Erdroy/Stationeers.Addons
---
```
And that is it :) 

**Note:** After publishing your addon for the first time, you have to copy `mods/MODNAME/About/About.xml` file back to your Visual Studio project (replace the file), as it now has `WorkshopHandle` tag and it is needed to update the adddon later!

___
***Stationeers.Addons** is not affiliated with RocketWerkz. All trademarks and registered trademarks are the property of their respective owners.*<br>
***Stationeers.Addons** (c) 2018-2020 Damian 'Erdroy' Korczowski & Contributors*
