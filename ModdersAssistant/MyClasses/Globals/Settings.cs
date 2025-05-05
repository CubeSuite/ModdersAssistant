using ModdersAssistant.MyClasses;
using ModdersAssistant.MyWindows;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;

namespace ModdersAssistant
{
    public class Settings
    {
        // How To Add A Setting:
        // 1. Give it a unique name in SettingNames
        // 2. Create it in the region below
        //   2.1 Default value must be an argument in the constructor to work properly
        //   2.2 Assign a category like this: General/Examples 
        // 3. Add it to GetAllSettings()
        // 4. Add it to the relevent SetSetting() function

        // Objects & Variables
        public static Settings userSettings = new Settings();
        public const string defaultCategory = "General";

        private static string iconColour = "#FFFFFF";

        #region Settings

        // General
        public BoolSetting logDebugMessages = new BoolSetting(false) {
            name = SettingNames.logDebugMessages,
            description = "Only enable if you're gathering information for a bug report.",
            category = "General",
            isHidden = false,
            OnValueChanged = (bool newValue) => {
                Log.logDebugToFile = newValue;
                Log.Debug($"Set Log.logDebugToFile to '{newValue}'");
            }
        };
        public ButtonSetting showLog = new ButtonSetting() {
            name = SettingNames.showLog,
            description = $"Opens the folder that contains {ProgramData.programName}'s log file.",
            category = "General",
            isHidden = false,
            buttonText = "Show In Explorer",
            OnClick = delegate () { Process.Start(ProgramData.FilePaths.logsFolder); }
        };
        public StringSetting thunderstoreUploadPage = new StringSetting("https://thunderstore.io/c/techtonica/create/") {
            name = SettingNames.thunderstoreUploadPage,
            description = "The URL of the Thunderstore upload page for your Community / Game",
            category = "General"
        };
        public StringSetting projectsFolder = new StringSetting() {
            name = SettingNames.projectsFolder,
            description = "Each project will get its own folder inside this one.",
            category = "General",
            isHidden = false
        };
        public ButtonSetting browseForProjectsFolder = new ButtonSetting() {
            name = SettingNames.browseForProjectsFolder,
            description = "Browse for the folder to store project folders inside.",
            category = "General",
            buttonText = "Browse",
            OnClick = delegate () {
                System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    userSettings.SetSetting(SettingNames.projectsFolder, dialog.SelectedPath, false);
                }
            }
        };
        public StringSetting gameFolder = new StringSetting() {
            name = SettingNames.gameFolder,
            description = "The root folder for the game you are modding.",
            category = "General"
        };
        public ButtonSetting browseForGameFolder = new ButtonSetting() {
            name = SettingNames.browseForGameFolder,
            description = "Browse for the root folder for the game you are modding.",
            category = "General",
            buttonText = "Browse",
            OnClick = delegate () {
                System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    userSettings.SetSetting(SettingNames.gameFolder, dialog.SelectedPath, false);
                }
            }
        };
        public StringSetting gameExecutable = new StringSetting() {
            name = SettingNames.gameExecutable,
            description = "The executable that launches the game you are modding.",
            category = "General"
        };
        public ButtonSetting browseForGameExecutable = new ButtonSetting() {
            name = SettingNames.browseForGameExecutable,
            description = "Browse for the executable that launches the game you are modding.",
            category = "General",
            buttonText = "Browse",
            OnClick = delegate () {
                System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog() {
                    Filter = "Executables (*.exe)|*.exe"
                };
                if (!string.IsNullOrEmpty(userSettings.gameFolder.value) && Directory.Exists(userSettings.gameFolder.value)) {
                    dialog.InitialDirectory = userSettings.gameFolder.value;
                }

                if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    userSettings.SetSetting(SettingNames.gameExecutable, dialog.FileName, false);
                }
            }
        };
        public StringSetting defaultGitHubRepo = new StringSetting() {
            name = SettingNames.defaultGitHubRepo,
            description = "If all your mods are kept in one repo, enter its link here",
            category = "General"
        };
        public StringSetting defaultSourceFolder = new StringSetting() {
            name = SettingNames.defaultSourceFolder,
            description = "If all your mods are kept in one solution, enter its path here",
            category = "General"
        };
        
        // README Template
        public StringSetting bannerImageLink = new StringSetting() {
            name = SettingNames.bannerImageLink,
            description = "The URL of your banner image",
            category = "README Generation",
            OnValueChanged = (string newValue) => {
                if (string.IsNullOrEmpty(newValue)) return;
                if(newValue.Contains("imgur") && !newValue.Contains("i.imgur")) {
                    GuiUtils.ShowWarningMessage("Invalid Image Link", "This image will not render on Thunderstore.\n" +
                                                "Open the link, right click the image, select 'Open image in new tab', copy the url of the new tab.");
                }
            }
        };
        public ButtonSetting editInstallationTemplate = new ButtonSetting() {
            name = SettingNames.editInstallationTemplate,
            description = "Opens the template to use for the Installation section of generated README.md files.",
            category = "README Generation",
            buttonText = "Edit",
            OnClick = delegate () {
                if (!File.Exists(ProgramData.FilePaths.installationTemplate)) {
                    StringUtils.GenerateInstallationTemplate();
                }

                Log.Debug("Opening Installation Section template");
                Process.Start(ProgramData.FilePaths.installationTemplate);
            }
        };
        public ButtonSetting editDisclaimerTemplate = new ButtonSetting() {
            name = SettingNames.editDisclaimerTemplate,
            description = "Opens the template to use for the Disclaimer section of generated README.md files.",
            category = "README Generation",
            buttonText = "Edit",
            OnClick = delegate () {
                if (!File.Exists(ProgramData.FilePaths.disclaimerTemplate)) {
                    StringUtils.GenerateDisclaimerTemplate();
                }

                Log.Debug("Opening Disclaimer Section template");
                Process.Start(ProgramData.FilePaths.disclaimerTemplate);
            }
        };

        // Backups
        public BoolSetting autoBackups = new BoolSetting(true) {
            name = SettingNames.autoBackup,
            description = $"Whether {ProgramData.programName} should automatically backup your data on a clean exit.",
            category = "Backups",
            isHidden = false
        };
        public StringSetting backupsFolder = new StringSetting(BackupManager.defaultBackupsFolder) {
            name = SettingNames.backupsFolder,
            description = "Where data backups should be stored. A OneDrive location is recommended for important data.",
            category = "Backups",
            isHidden = false
        };
        public ButtonSetting browseForBackupsFolder = new ButtonSetting() {
            name = SettingNames.browseForBackupsFolder,
            description = "Browse for a folder to store backups in.",
            category = "Backups",
            buttonText = "Browse",
            OnClick = delegate () {
                System.Windows.Forms.FolderBrowserDialog browser = new System.Windows.Forms.FolderBrowserDialog();
                if (browser.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    userSettings.SetSetting(SettingNames.backupsFolder, browser.SelectedPath, false);
                }
            }
        };
        public IntSetting numBackups = new IntSetting(5) {
            name = SettingNames.numBackups,
            description = "The maximum number of data backups to keep.",
            category = "Backups",
            min = 1,
            max = 10
        };
        public ButtonSetting openBackups = new ButtonSetting() {
            name = SettingNames.openBackups,
            description = "Opens the Data Backup Management Window.",
            category = "Backups",
            buttonText = "Open Backup Manager",
            OnClick = delegate () { BackupManagementWindow.ShowBackupManagerWindow(); }
        };

        // Theme
        public ButtonSetting restoreDefaultTheme = new ButtonSetting() {
            name = SettingNames.restoreDefaultTheme,
            description = "Resets the theme to the default colours.",
            category = "Theme",
            buttonText = "Restore Default Theme",
            OnClick = delegate () {
                if (GuiUtils.GetUserConfirmation("Restore Default Theme?", "Are you sure you want to restore the default theme? This cannot be undone.")) {
                    userSettings.dimBackground.RestoreDefault();
                    userSettings.normalBackground.RestoreDefault();
                    userSettings.brightBackground.RestoreDefault();
                    userSettings.uiBackground.RestoreDefault();
                    userSettings.accentColour.RestoreDefault();
                    userSettings.textColour.RestoreDefault();
                    Save();
                    SettingsChanged?.Invoke(null, new SettingChangedEventArgs() { changeFromGUI = false });
                    LoadTheme();
                }
            }
        };
        public ColourSetting dimBackground = new ColourSetting(Color.FromRgb(35, 35, 35)) {
            name = SettingNames.dimBackground,
            description = $"The background colour of {ProgramData.programName}.",
            category = "Theme",
            isHidden = false,
        };
        public ColourSetting normalBackground = new ColourSetting(Color.FromRgb(53, 53, 53)) {
            name = SettingNames.normalBackground,
            description = "Should be slightly brighter than 'Dim Background'.",
            category = "Theme"
        };
        public ColourSetting brightBackground = new ColourSetting(Color.FromRgb(71, 71, 71)) {
            name = SettingNames.brightBackground,
            description = "Should be slightly brighter than 'Normal Background'.",
            category = "Theme"
        };
        public ColourSetting uiBackground = new ColourSetting(Color.FromRgb(89, 89, 89)) {
            name = SettingNames.uiBackground,
            description = "Should be slightly brighter than 'Bright Background'.",
            category = "Theme"
        };
        public ColourSetting accentColour = new ColourSetting(Colors.DodgerBlue) {
            name = SettingNames.accentColour,
            description = $"The accent colour of {ProgramData.programName}.",
            category = "Theme"
        };
        public ColourSetting textColour = new ColourSetting(Colors.White) {
            name = SettingNames.textColour,
            description = $"The text and icon colour of {ProgramData.programName}.",
            category = "Theme"
        };
        public ColourSetting goodTextColour = new ColourSetting(Colors.Lime) {
            name = SettingNames.goodTextColour,
            description = "The colour of text that says something good, e.g. 'Yes'",
            category = "Theme"
        };
        public ColourSetting badTextColour = new ColourSetting(Colors.Red) {
            name = SettingNames.badTextColour,
            description = "The colour of text that says something bad, e.g. 'No'",
            category = "Theme"
        };

        // Hidden - Put in default category
        public BoolSetting isFirstTimeLaunch = new BoolSetting(true) {
            name = SettingNames.isFirstTimeLaunch,
            description = "Only true the first time the program is run.",
            category = defaultCategory,
            isHidden = true
        };
        public BoolSetting descriptionWrapText = new BoolSetting(true) {
            name = SettingNames.descriptionWrapText,
            description = "Whether to wrap text for the descriptionBox on a ProjectPanel",
            category = defaultCategory,
            isHidden = true
        };

        #endregion

        // Constructor

        public Settings() {
            RestoreDefaults(false);
        }

        // Custom Events

        public static event EventHandler<SettingChangedEventArgs> SettingsChanged;

        // Public Functions

        public List<Setting> GetAllSettings() {
            return new List<Setting>() {
                // General
                logDebugMessages,
                showLog,
                thunderstoreUploadPage,
                projectsFolder,
                browseForProjectsFolder,
                gameFolder,
                browseForGameFolder,
                gameExecutable,
                browseForGameExecutable,
                defaultGitHubRepo,
                defaultSourceFolder,
                
                // README Template
                bannerImageLink,
                editInstallationTemplate,
                editDisclaimerTemplate,

                // Backups
                autoBackups,
                backupsFolder,
                browseForBackupsFolder,
                numBackups,
                openBackups,

                // Theme
                restoreDefaultTheme,
                dimBackground,
                normalBackground,
                brightBackground,
                uiBackground,
                accentColour,
                textColour,
                goodTextColour,
                badTextColour,

                // Hidden
                isFirstTimeLaunch,
            };
        }

        public List<Setting> GetSettingsInCategory(string category) {
            return GetAllSettings().Where(setting => setting.category.Contains(category)).ToList();
        }

        public void RestoreDefaults(bool shouldSave = true) {
            foreach (Setting setting in GetAllSettings()) {
                if (setting.name == SettingNames.isFirstTimeLaunch) continue;
                setting.RestoreDefault();
            }

            if (shouldSave) Save();
        }

        public void SetSetting(string name, string value, bool changeFromGUI = true) {
            switch (name) {
                case SettingNames.thunderstoreUploadPage: thunderstoreUploadPage.value = value; break;
                case SettingNames.projectsFolder: projectsFolder.value = value; break;
                case SettingNames.gameFolder: gameFolder.value = value; break;
                case SettingNames.gameExecutable: gameExecutable.value = value; break;
                case SettingNames.bannerImageLink: bannerImageLink.value = value; break;
                case SettingNames.backupsFolder: backupsFolder.value = value; break;
                case SettingNames.defaultGitHubRepo: defaultGitHubRepo.value = value; break;
                case SettingNames.defaultSourceFolder: defaultSourceFolder.value = value; break;
                default:
                    Log.Error($"Could not find the StringSetting named '{name}'");
                    return;
            }

            Log.Info($"Set Setting '{name}' to '{value}'");
            Save();
            SettingsChanged?.Invoke(this, new SettingChangedEventArgs() { changeFromGUI = changeFromGUI });
        }

        public void SetSetting(string name, int value, bool changeFromGUI = true) {
            switch (name) {
                case SettingNames.numBackups: numBackups.value = value; break;
                default:
                    Log.Error($"Could not find the IntSetting named '{name}'");
                    return;
            }

            Log.Info($"Set Setting '{name}' to '{value}'");
            Save();
            SettingsChanged?.Invoke(this, new SettingChangedEventArgs() { changeFromGUI = changeFromGUI });
        }

        public void SetSetting(string name, bool value, bool changeFromGUI = true) {
            switch (name) {
                case SettingNames.logDebugMessages: logDebugMessages.value = value; break;
                case SettingNames.isFirstTimeLaunch: isFirstTimeLaunch.value = value; break;
                case SettingNames.descriptionWrapText: descriptionWrapText.value = value; break;
                default:
                    Log.Error($"Could not find the BoolSetting named '{name}'");
                    return;
            }

            Log.Info($"Set Setting '{name}' to '{value}'");
            Save();
            SettingsChanged?.Invoke(this, new SettingChangedEventArgs() { changeFromGUI = changeFromGUI });
        }

        public void SetSetting(string name, Color value, bool changeFromGUI = true) {
            switch (name) {
                case SettingNames.dimBackground: dimBackground.value = value; break;
                case SettingNames.normalBackground: normalBackground.value = value; break;
                case SettingNames.brightBackground: brightBackground.value = value; break;
                case SettingNames.uiBackground: uiBackground.value = value; break;
                case SettingNames.accentColour: accentColour.value = value; break;
                case SettingNames.textColour: textColour.value = value; break;
                default:
                    Log.Error($"Could not find the ColourSetting named '{name}'");
                    return;
            }

            Log.Info($"Set Setting '{name}' to '{value}'");
            Save();
            LoadTheme();
            SettingsChanged?.Invoke(this, new SettingChangedEventArgs() { changeFromGUI = changeFromGUI });
        }

        public static void LoadTheme() {
            Application.Current.Resources["dimBackgroundBrush"] = new SolidColorBrush(userSettings.dimBackground.value);
            Application.Current.Resources["backgroundBrush"] = new SolidColorBrush(userSettings.normalBackground.value);
            Application.Current.Resources["brightBackgroundBrush"] = new SolidColorBrush(userSettings.brightBackground.value);
            Application.Current.Resources["uiBackgroundBrush"] = new SolidColorBrush(userSettings.uiBackground.value);
            Application.Current.Resources["accentBrush"] = new SolidColorBrush(userSettings.accentColour.value);
            Application.Current.Resources["textBrush"] = new SolidColorBrush(userSettings.textColour.value);

            Application.Current.Resources["accentColour"] = userSettings.accentColour.value;
            RecolourIcons();
        }

        // Private Functions

        private bool ValidateNames() {
            List<string> names = GetAllSettings().Select(setting => setting.name).ToList();
            List<string> uniqueNames = names.Distinct().ToList();
            bool passed = names.Count == uniqueNames.Count;
            if (!passed) {
                for (int i = 0; i < names.Count; i++) {
                    for (int j = 0; j < names.Count; j++) {
                        if (i == j) continue;
                        if (names[i] == names[j]) Log.Warning($"Setting name '{names[i]}' is not unique");
                    }
                }
            }

            return passed;
        }

        private static void RecolourIcons() {
            string newColourHex = StringUtils.ColourToHex(userSettings.textColour.value);
            if (newColourHex == iconColour) return;

            List<string> svgsToRecolour = new List<string>();
            svgsToRecolour.AddRange(Directory.GetFiles($"{ProgramData.FilePaths.resourcesFolder}\\ControlBox"));
            svgsToRecolour.AddRange(Directory.GetFiles($"{ProgramData.FilePaths.resourcesFolder}\\GUI"));
            foreach (string svg in svgsToRecolour) {
                Log.Debug($"Updating {svg} to {newColourHex}");
                string OldText = File.ReadAllText(svg);
                Log.Debug($"Read svg file");

                string NewText = OldText.Replace(iconColour.ToLower(), newColourHex.ToLower());

                File.WriteAllText(svg, NewText);
                Log.Debug($"Wrote svg file");
            }

            iconColour = StringUtils.ColourToHex(userSettings.textColour.value);
            MainWindow.current.controlBox.RefreshIcons();
        }

        // Data Functions

        public static void Save() {
            userSettings.ValidateNames();
            string json = JsonConvert.SerializeObject(userSettings, Formatting.Indented);
            File.WriteAllText(ProgramData.FilePaths.settingsFile, json);

            Log.Info("Settings Saved");
        }

        public static void Load() {
            if (!File.Exists(ProgramData.FilePaths.settingsFile)) {
                userSettings = new Settings();
                Save();
            }

            string json = File.ReadAllText(ProgramData.FilePaths.settingsFile);
            userSettings = JsonConvert.DeserializeObject<Settings>(json);
            iconColour = StringUtils.ColourToHex(userSettings.textColour.value);
            LoadTheme();

            Log.Info("Settings Loaded");
        }
    }

    public static class SettingNames
    {
        // General
        public const string logDebugMessages = "Log Debug Messages";
        public const string showLog = "Show Log In Explorer";
        public const string thunderstoreUploadPage = "Thunderstore Upload Page";
        public const string projectsFolder = "Projects Folder";
        public const string browseForProjectsFolder = "Browse For Projects Folder";
        public const string gameFolder = "Game Folder";
        public const string browseForGameFolder = "Browse For Game Folder";
        public const string gameExecutable = "Game Executable";
        public const string browseForGameExecutable = "Browse For Game Executable";
        public const string defaultGitHubRepo = "Default GitHub Repo";
        public const string defaultSourceFolder = "Default Source Folder";
        
        // README Template
        public const string bannerImageLink = "Banner Image Link";
        public const string editInstallationTemplate = "Edit Installation Section Template";
        public const string editDisclaimerTemplate = "Edit Disclaimer Section Template";

        // Backups
        public const string autoBackup = "Perform Automatic Backups";
        public const string backupsFolder = "Backups Folder";
        public const string browseForBackupsFolder = "Browse For Backups Folder";
        public const string numBackups = "Backup Count";
        public const string openBackups = "Backups Manager";

        // Theme
        public const string restoreDefaultTheme = "Restore Default Theme";
        public const string dimBackground = "Dim Background Colour";
        public const string normalBackground = "Normal Background Colour";
        public const string brightBackground = "Bright Background Colour";
        public const string uiBackground = "UI Background Colour";
        public const string accentColour = "Accent Colour";
        public const string textColour = "Text Colour";
        public const string goodTextColour = "Good Text Colour";
        public const string badTextColour = "Bad Text Colour";

        // Hidden
        public const string isFirstTimeLaunch = "Is First Time Launch";
        public const string descriptionWrapText = "Wrap Text For Description";
    }

    public class SettingChangedEventArgs : EventArgs
    {
        public bool changeFromGUI;
    }
}
