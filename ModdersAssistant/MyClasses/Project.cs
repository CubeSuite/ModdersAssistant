using ModdersAssistant.MyClasses;
using ModdersAssistant.MyClasses.Globals;
using ModdersAssistant.MyWindows;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;

namespace ModdersAssistant.MyClasses
{
    public class Project{
        public int id;
        public string name;
        public Version version = Version.GetDefault();
        public Version lastReleaseVersion = new Version(0, 0, 0);
        public string tagline;
        public string description;
        public string changelogHistory;
        public string repo;
        public string thunderstoreLink;
        public List<string> dependencies = new List<string>();
        public List<string> credits = new List<string>();
        public List<Ticket> tickets = new List<Ticket>();
        public List<Screenshot> screenshots = new List<Screenshot>();
        public List<string> gameFolderFiles = new List<string>();

        public string sourceFolder;
        public string modFilesFolder;
        public string screenshotsFolder;
        public string resourcesFolder;
        public bool hasConfigFile;
        public bool hasIconFile;
        public string latestZipFile;

        public bool isReleased => version == lastReleaseVersion;


        // Constructors

        public Project() { }
        public Project FromJson(string json) {
            return JsonConvert.DeserializeObject<Project>(json);
        }

        // Public Functions

        public override string ToString() {
            return $"Project #{id} '{name}'";
        }

        public string ToJson(bool indented = true) {
            return JsonConvert.SerializeObject(this, indented ? Formatting.Indented : Formatting.None);
        }

        public void Rename(string newName) {
            Directory.Move(GetFolder(), $"{Settings.userSettings.projectsFolder.value}/{newName}");

            string oldPluginsFolder = $"{Settings.userSettings.projectsFolder.value}/{newName}/Mod Files/plugins/{GetFileSafeName()}";
            Log.Debug($"oldPluginsFolder: {oldPluginsFolder}");
            if (Directory.Exists(oldPluginsFolder)) {
                string newPath = oldPluginsFolder.Replace(GetFileSafeName(), StringUtils.GetFileSafeName(newName));
                if (oldPluginsFolder != newPath) {
                    Directory.Move(oldPluginsFolder, newPath);
                }
            }

            sourceFolder = sourceFolder.Replace(name, newName);
            modFilesFolder = modFilesFolder.Replace(name, newName);
            screenshotsFolder = screenshotsFolder.Replace(name, newName);
            resourcesFolder = resourcesFolder.Replace(name, newName);

            if(!string.IsNullOrEmpty(latestZipFile)) latestZipFile = latestZipFile.Replace(name, newName);

            name = newName;
            WriteManifestJson();

            //string newSourceFolder = sourceFolder.Replace(name, newName);
            //string newModFilesFolder = modFilesFolder.Replace(name, newName);
            //string newScreenshotsFolder = screenshotsFolder.Replace(name, newName);
            //string newResourcesFolder = resourcesFolder.Replace(name, newName);
            //string newLatestZipFile = latestZipFile.Replace(name, newName);

            //Directory.Move(sourceFolder, newSourceFolder);
            //Directory.Move(modFilesFolder, newModFilesFolder);
            //Directory.Move(screenshotsFolder, newScreenshotsFolder);
            //Directory.Move(resourcesFolder, newResourcesFolder);
            //File.Move()
        }

        public string GetFileSafeName() {
            return StringUtils.GetFileSafeName(name);
        }

        public string GetFolder() {
            string projectsFolder = Settings.userSettings.projectsFolder.value;
            if (!Directory.Exists(projectsFolder)) {
                Log.Error($"Cannot get folder for Project {this}. Parent folder '{projectsFolder}' does not exist");
                return "";
            }

            return $"{projectsFolder}/{name}";
        }

        public void CreateFolders() {
            string projectFolder = GetFolder();
            if (string.IsNullOrEmpty(projectFolder)) {
                Log.Error("Cannot create folder for null Project folder");
                return;
            }

            sourceFolder = $"{projectFolder}/Source";
            Directory.CreateDirectory(sourceFolder);
            Log.Debug($"Created Source folder for {this}: '{sourceFolder}'");

            modFilesFolder = $"{projectFolder}/Mod Files";
            Directory.CreateDirectory(modFilesFolder);
            Directory.CreateDirectory($"{modFilesFolder}/plugins");
            Directory.CreateDirectory($"{modFilesFolder}/plugins/{GetFileSafeName()}");
            Log.Debug($"Created Mod Files folders for {this}: '{modFilesFolder}'");

            screenshotsFolder = $"{projectFolder}/Screenshots";
            Directory.CreateDirectory(screenshotsFolder);
            Log.Debug($"Created Screenshots folder for {this}: '{screenshotsFolder}'");

            resourcesFolder = $"{projectFolder}/Resources";
            Directory.CreateDirectory(resourcesFolder);
            Log.Debug($"Created Resources folder for {this}: '{resourcesFolder}'");
        }

        public void OpenInCodeEditor() {
            string slnFile;
            if(!FindSlnFile(sourceFolder, out slnFile)) {
                GuiUtils.ShowErrorMessage("Couldn't find .sln File", $"{ProgramData.programName} couldn't find a .sln file in {name}'s 'Source' folder.\n" +
                                                                     $"Please move this mod's source code to the 'Source' folder and try again.");
                return;
            }

            Process.Start(slnFile);
        }

        public List<Ticket> GetSortedTickets() {
            List<Ticket> sorted = new List<Ticket>();
            sorted.AddRange(tickets.Where(ticket => ticket.type == TicketType.Bug && !ticket.complete).OrderBy(ticket => ticket.title));
            sorted.AddRange(tickets.Where(ticket => ticket.type == TicketType.Change && !ticket.complete).OrderBy(ticket => ticket.title));
            sorted.AddRange(tickets.Where(ticket => ticket.type == TicketType.NewFeature && !ticket.complete).OrderBy(ticket => ticket.title));
            sorted.AddRange(tickets.Where(ticket => ticket.type == TicketType.Bug && ticket.complete).OrderBy(ticket => ticket.title));
            sorted.AddRange(tickets.Where(ticket => ticket.type == TicketType.Change && ticket.complete).OrderBy(ticket => ticket.title));
            sorted.AddRange(tickets.Where(ticket => ticket.type == TicketType.NewFeature && ticket.complete).OrderBy(ticket => ticket.title));
            return sorted;
        }

        public void WriteManifestJson() {
            if(dependencies.Count != 0) {
                dependencies = dependencies.Where(mod => !string.IsNullOrEmpty(mod)).ToList();
            }

            Manifest manifest = new Manifest() {
                name = GetFileSafeName(),
                description = tagline,
                version_number = version.ToString().Replace("V", ""),
                dependencies = dependencies,
                website_url = string.IsNullOrEmpty(repo) ? "" : repo
            };
            string path = $"{modFilesFolder}\\manifest.json";
            string json = JsonConvert.SerializeObject(manifest, Formatting.Indented);
            File.WriteAllText(path, json);

            File.SetCreationTime(path, DateTime.Now);
            File.SetLastWriteTime(path, DateTime.Now);
            File.SetLastAccessTime(path, DateTime.Now);
        }

        public void WriteReadMePlaceHolder() {
            File.WriteAllText($"{modFilesFolder}\\README.md", "Contents will be written when you click 'Prepare Upload'");
        }

        public void WriteReadMe() {
            string path = $"{modFilesFolder}\\README.md";
            string markdown = MarkdownWriter.GetMarkdownForProject(this);
            File.WriteAllText(path, markdown);

            File.SetCreationTime(path, DateTime.Now);
            File.SetLastWriteTime(path, DateTime.Now);
            File.SetLastAccessTime(path, DateTime.Now);
        }

        public string GenerateDiscordMessage() {
            string message;
            if(version == Version.GetDefault()) {
                message = $"New Mod Release - [{name}]({thunderstoreLink})\n\n{tagline}";
            }
            else {
                message = $"[{name}]({thunderstoreLink}) has been updated to {version}!\n\n" +
                          $"Changes:\n" +
                          $"{MarkdownWriter.GetChangelog(this)}";
            }

            string suffix = StringUtils.GetDiscordMessageTemplate();
            if (!string.IsNullOrEmpty(suffix)) {
                message += $"\n\n{suffix}";
            }

            return message;
        }

        public async Task SearchForModFiles() {
            if (!Directory.Exists(Settings.userSettings.gameFolder.value)) {
                if (!ProgramData.doneGameFolderWarning) {
                    ProgramData.doneGameFolderWarning = true;
                    GuiUtils.ShowWarningMessage("Invalid Game Folder Setting", "You need to update your Game Folder setting to a folder that exists.");
                }

                return;
            }

            string pluginsFolder = $"{Settings.userSettings.gameFolder.value}\\BepInEx\\plugins";
            string configFolder = $"{Settings.userSettings.gameFolder.value}\\BepInEx\\config";

            await Task.Run(() => {
                foreach (string file in Directory.GetFiles(pluginsFolder)) {
                    if (file.Contains(GetFileSafeName())) {
                        string newPath = $"{modFilesFolder}/plugins/{GetFileSafeName()}/{Path.GetFileName(file)}";
                        File.Copy(file, newPath, true);
                        if (!gameFolderFiles.Contains(file)) {
                            gameFolderFiles.Add(file);
                        }
                    }
                }

                foreach (string folder in Directory.GetDirectories(pluginsFolder)) {
                    if (folder.Contains(GetFileSafeName())) {
                        foreach (string file in Directory.GetFiles(folder)) {
                            if (file.Contains(GetFileSafeName())) {
                                Directory.CreateDirectory($"{modFilesFolder}/plugins");
                                Directory.CreateDirectory($"{modFilesFolder}/plugins/{GetFileSafeName()}");
                                string newPath = $"{modFilesFolder}/plugins/{GetFileSafeName()}/{Path.GetFileName(file)}";
                                File.Copy(file, newPath, true);
                                if (!gameFolderFiles.Contains(file)) {
                                    gameFolderFiles.Add(file);
                                }
                            }
                        }
                    }
                }

                foreach (string file in Directory.GetFiles(configFolder)) {
                    if (file.Contains(GetFileSafeName())) {
                        hasConfigFile = true;
                        Directory.CreateDirectory($"{modFilesFolder}/config");
                        string newPath = $"{modFilesFolder}/config/{Path.GetFileName(file)}";
                        File.Copy(file, newPath, true);
                        if (!gameFolderFiles.Contains(file)) {
                            gameFolderFiles.Add(file);
                        }
                    }
                }

                foreach (string file in Directory.GetFiles(screenshotsFolder)) {
                    string name = Path.GetFileName(file);
                    if (screenshots.Where(screenshot => screenshot.name == name).Count() == 0) {
                        screenshots.Add(new Screenshot() {
                            name = name,
                            url = ""
                        });
                    }
                }
            });
        }

        public async Task<bool> RefreshFiles() {
            if(!FindSlnFile(sourceFolder, out string slnPath)) {
                GuiUtils.ShowErrorMessage("Can't Auto Refresh Files", "Modder's Assistant couldn't find a .sln file in this Project's 'Source' folder,\n" +
                                                                      "so it can't refresh the mod files.");
                return false;
            }

            if(hasConfigFile && !File.Exists(Settings.userSettings.gameExecutable.value)) {
                GuiUtils.ShowErrorMessage("Can't Auto Refresh Files", "This project has a config file that can't be refreshed as the setting 'Game Executable' has not been set correctly.");
                return false;
            }

            await SearchForModFiles();
            DeleteFromGameFolder();
            ClearModFiles(modFilesFolder);
            RebuildPlugin(slnPath);
            
            if (hasConfigFile) {
                RegenerateConfigFile();
                while (!Directory.Exists($"{modFilesFolder}\\config")) {
                    await Task.Delay(1000);
                    await SearchForModFiles();
                }
            }
            else {
                await SearchForModFiles();
            }

            return true;
        }

        public bool ZipModFiles() {
            try {
                Log.Debug($"Zipping Mod Files for {this}");
                string zipPath = $"{ProgramData.FilePaths.dataFolder}/{GetFileSafeName()}-{version}.zip";
                if (File.Exists(zipPath)) File.Delete(zipPath);
                ZipFile.CreateFromDirectory(modFilesFolder, zipPath);

                string newPath = zipPath.Replace(ProgramData.FilePaths.dataFolder, modFilesFolder);
                if (File.Exists(newPath)) File.Delete(newPath);
                File.Move(zipPath, newPath);
                latestZipFile = newPath;
                Log.Info("Successfully created zip file");
                return true;
            }
            catch (Exception e) {
                string error = "Error occurred while creating zip file: " + e.Message;
                Log.Error(error);
                DebugUtils.CrashIfDebug(error);
                if (!ProgramData.isDebugBuild) {
                    GuiUtils.ShowErrorMessage("Error Occurred While Zipping Files", e.Message);
                }
            }

            return false;
        }

        // Private Functions

        private bool FindSlnFile(string folder, out string path) {
            string[] files = Directory.GetFiles(folder);
            foreach(string file in files) {
                if (file.EndsWith(".sln")) {
                    path = file;
                    return true;
                }
            }

            string[] folders = Directory.GetDirectories(folder);
            foreach(string subFolder in folders) {
                if(FindSlnFile(subFolder, out path)) return true;
            }

            path = "";
            return false;
        }

        // Refresh Mod Files

        private void DeleteFromGameFolder() {
            foreach(string file in gameFolderFiles) {
                File.Delete(file);
            }

            gameFolderFiles.Clear();
        }

        private void ClearModFiles(string folder) {
            string[] files = Directory.GetFiles(folder);
            foreach(string file in files) {
                if (file.EndsWith("icon.png") || file.EndsWith("manifest.json") || file.EndsWith("README.md")) continue;
                File.Delete(file);
            }

            foreach(string subFolder in Directory.GetDirectories(folder)) {
                ClearModFiles(subFolder);
                Directory.Delete(subFolder);
            }
        }

        private void RebuildPlugin(string slnPath) {
            Process.Start(new ProcessStartInfo() {
                FileName = "dotnet",
                Arguments = $"build \"{slnPath}\" --configuration Release --no-incremental",
                UseShellExecute = true,
                CreateNoWindow = true
            });
        }

        private void RegenerateConfigFile() {
            GuiUtils.ShowInfoMessage("Refreshing Config File", "Modder's Assistant will now launch the game to refresh the config file.\n" +
                                                               "Please close this popup and then quit the game at the title screen.");
            Process.Start(Settings.userSettings.gameExecutable.value);
        }
    }
}
