using ModdersAssistant.MyWindows;
using ModdersAssistant.MyWindows.GetWindows;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ModdersAssistant
{
    public static class GuiUtils
    {
        public static bool GetUserConfirmation(string title, string description) {
            return GetYesNoWindow.GetYesNo(title, description);
        }

        public static string GetStringFromUser(string title, string hint) {
            return GetStringWindow.GetString(title, hint);
        }

        public static int GetIntFromUser(string title, int min, int max, int? defaultValue = null) {
            return GetIntWindow.GetInt(title, min, max, defaultValue);
        }

        public static void ShowInfoMessage(string title, string description, string closeButtonText = "Close") {
            WarningWindow.ShowInfo(title, description, closeButtonText);
        }

        public static void ShowWarningMessage(string title, string description, string closeButtonText = "Close") {
            WarningWindow.ShowWarning(title, description, closeButtonText);
        }

        public static void ShowErrorMessage(string title, string description, string closeButtonText = "Close") {
            WarningWindow.ShowError(title, description, closeButtonText);
        }
    }

    public static partial class StringUtils
    {
        public static string ColourToHex(Color color) {
            return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }

        public static void GenerateInstallationTemplate() {
            string template = @"If you are using Techtonica Mod Loader, you can ignore these steps.

Note, this mod requires use of the BepInEx Update function. If you have not already done so for another mod, follow these instructions:
1. Find your game install folder.
2. Navigate to BepInEx\config.
3. Open BepInEx.cfg.
4. Find the setting ""HideGameManagerObject"".
5. Set it to ""true"".
6. Save & close.

### Techtonica Mod Loader Installation

You can download the Techtonica Mod Loader from [here](https://github.com/CubeSuite/TechtonicaModLoader/releases) and use that to install this mod.

### Manual Install Instructions

Note: If you are playing on Gamepass, your game version is likely behind the steam version. Please check the version compatibility chart below.

Your game folder is likely in one of these places:  
    • Steam: (A-Z):/steam/steamapps/common/Techtonica  
    • Gamepass: (A-Z):/XboxGames/Techtonica/Content  
    • Gamepass: Could also be in C:/Program Data/WindowsApps  

1. Download BepInEx v5.4.21 from [here](https://github.com/BepInEx/BepInEx/releases)
1. Follow the installation instructions [here](https://docs.bepinex.dev/articles/user_guide/installation/index.html)
1. Download and install any dependencies.
1. Extract the contents of the .zip file for this mod.
1. Drag the ""BepInEx"" folder into your game folder.
1. Change config options. ";
            File.WriteAllText(ProgramData.FilePaths.installationTemplate, template);
            Log.Debug("Generated default Installation section template");
        }

        public static string GetInstallationTemplate() {
            if(!File.Exists(ProgramData.FilePaths.installationTemplate)) GenerateInstallationTemplate();
            return File.ReadAllText(ProgramData.FilePaths.installationTemplate);
        }

        public static void GenerateDisclaimerTemplate() {
            string template = @"Note: NEW Games must be loaded, saved, and reloaded for mods to take effect. Existing saves will auto-apply mods. 
Please be sure to backup your saves before using mods: AppData\LocalLow\Fire Hose Games\Techtonica 
USE AT YOUR OWN RISK! Techtonica Devs do not provide support for Mods, and cannot recover saves damaged by mod usage.

Some assets may come from Techtonica or from the website created and owned by Fire Hose Games, who hold the copyright of Techtonica. All trademarks and registered trademarks present in any images are proprietary to Fire Hose Games.";
            File.WriteAllText(ProgramData.FilePaths.disclaimerTemplate, template);
            Log.Debug("Generated default Disclaimer section template");
        }

        public static string GetDisclaimerTemplate() {
            if (!File.Exists(ProgramData.FilePaths.disclaimerTemplate)) GenerateDisclaimerTemplate();
            return File.ReadAllText(ProgramData.FilePaths.disclaimerTemplate);
        }

        public static void GenerateDiscordMessageTemplate() {
            string template = @"Get [Techtonica Mod Loader](https://github.com/CubeSuite/TechtonicaModLoader/releases)";
            File.WriteAllText(ProgramData.FilePaths.discordMessageTemplate, template);
            Log.Debug("Generated default Discord message template");
        }

        public static string GetDiscordMessageTemplate() {
            if (!File.Exists(ProgramData.FilePaths.discordMessageTemplate)) GenerateDiscordMessageTemplate();
            return File.ReadAllText(ProgramData.FilePaths.discordMessageTemplate);
        }

        public static string GetFileSafeName(string name) {
            if (string.IsNullOrEmpty(name)) {
                Log.Error($"Cannot get safe name for '{name}' - name is null or empty");
                return "Unnamed";
            }

            StringBuilder fileSafeName = new StringBuilder();

            foreach (char letter in name) {
                if (char.IsLetterOrDigit(letter) || letter == '-' || letter == '_') {
                    fileSafeName.Append(letter);
                }
            }

            return fileSafeName.ToString();
        }
    }

    public static class DebugUtils
    {
        public static void CrashIfDebug(string error) {
            if (!ProgramData.isDebugBuild) return;
            throw new Exception(error);
        }
    }
}
