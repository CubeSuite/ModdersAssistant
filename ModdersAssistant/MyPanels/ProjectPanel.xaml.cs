using ModdersAssistant.MyClasses;
using ModdersAssistant.MyWindows;
using ModdersAssistant.MyWindows.GetWindows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace ModdersAssistant.MyPanels
{
    /// <summary>
    /// Interaction logic for ProjectPanel.xaml
    /// </summary>
    public partial class ProjectPanel : UserControl
    {
        public ProjectPanel() {
            InitializeComponent();
        }

        public ProjectPanel(Project projectToShow) {
            InitializeComponent();
            ShowProject(projectToShow);
        }

        // Objects & Variables
        public Project project;
        public DispatcherTimer fileSearchTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(5) };
        
        private bool finishedLoading = false;

        // Custom Events

        public event EventHandler ProjectNameEdited;

        // Title GUI Events

        private void OnEditProjectNameClicked(object sender, EventArgs e) {
            if(project.version.ToString() != "V1.0.0" || project.isReleased) {
                if (!GuiUtils.GetUserConfirmation("Rename Mod", "Are you sure you want to rename this mod?\nThunderstore will upload it as a new mod and not an update.")) return;
            }

            string newName = GuiUtils.GetStringFromUser("Enter New Project Name:", "New Name...");

            if (string.IsNullOrEmpty(newName)) {
                GuiUtils.ShowErrorMessage("Invalid Name", "Your project's name cannot be blank.");
                return;
            }

            if (!ProjectManager.IsNameUnique(newName)) {
                GuiUtils.ShowErrorMessage("Invalid Name", "This name is not unique.");
                return;
            }

            if(newName != GetStringWindow.canceledInput) {
                Log.Debug($"Updating name of {project} to '{newName}'");
                nameLabel.Content = newName;
                project.Rename(newName);
                ProjectManager.UpdateProject(project);
                ProjectNameEdited?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnMajorBoxTextEdited(object sender, EventArgs e) {
            if (!finishedLoading) return;
            Log.Debug($"Updating major version number for {project} to '{majorBox.Input}'");
            if (int.TryParse(majorBox.Input, out int major)) {
                project.version.major = major;
                project.WriteManifestJson();
                ProjectManager.UpdateProject(project);
                releasedLabel.Visibility = project.isReleased ? Visibility.Visible : Visibility.Hidden;
            }
            else {
                GuiUtils.ShowErrorMessage("Couldn't Parse Major Version Number", $"{ProgramData.programName} couldn't parse '{majorBox.Input}' into an integer. Please try again.");
                majorBox.Input = project.version.major.ToString();
            }
        }

        private void OnMinorBoxTextEdited(object sender, EventArgs e) {
            if (!finishedLoading) return;
            Log.Debug($"Updating minor version number for {project} to '{minorBox.Input}'");
            if (int.TryParse(minorBox.Input, out int minor)) {
                project.version.minor = minor;
                project.WriteManifestJson();
                ProjectManager.UpdateProject(project);
                releasedLabel.Visibility = project.isReleased ? Visibility.Visible : Visibility.Hidden;
            }
            else {
                GuiUtils.ShowErrorMessage("Couldn't Parse Minor Version Number", $"{ProgramData.programName} couldn't parse '{minorBox.Input}' into an integer. Please try again.");
                minorBox.Input = project.version.minor.ToString();
            }
        }

        private void OnPatchBoxTextEdited(object sender, EventArgs e) {
            if (!finishedLoading) return;
            Log.Debug($"Updating patch version number for {project} to '{patchBox.Input}'");
            if (int.TryParse(patchBox.Input, out int patch)) {
                project.version.patch = patch;
                project.WriteManifestJson();
                ProjectManager.UpdateProject(project);
                releasedLabel.Visibility = project.isReleased ? Visibility.Visible : Visibility.Hidden;
            }
            else {
                GuiUtils.ShowErrorMessage("Couldn't Parse Patch Version Number", $"{ProgramData.programName} couldn't parse '{patchBox.Input}' into an integer. Please try again.");
                patchBox.Input = project.version.patch.ToString();
            }
        }

        private void OnOpenInCodeEditorClicked(object sender, EventArgs e) {
            project.OpenInCodeEditor();
        }

        private void OnEditChangelogHistoryClicked(object sender, EventArgs e) {
            ChangeLogHistoryWindow.EditChangelogHistory(ref project);
        }

        // Main Input Form Events

        private void OnTaglineBoxTextChanged(object sender, EventArgs e) {
            if (!finishedLoading) return;
            Log.Debug($"Udpating tagline for {project} to {tagLineBox.Input}");
            project.tagline = tagLineBox.Input;
            project.WriteManifestJson();
            ProjectManager.UpdateProject(project);
        }

        private void OnDependenciesBoxTextChanged(object sender, EventArgs e) {
            if (!finishedLoading) return;
            Log.Debug($"Updating dependencies for {project} to '{dependenciesBox.Input}'");
            try {
                project.dependencies = dependenciesBox.Input.Replace(" ", "").Split(',').ToList();
                project.WriteManifestJson();
                ProjectManager.UpdateProject(project);
            }
            catch (Exception ex) {
                string error = $"Error occurred while parsing dependencies: {ex.Message}";
                Log.Error(error);
                DebugUtils.CrashIfDebug(error);

                if (!ProgramData.isDebugBuild) {
                    GuiUtils.ShowErrorMessage("Couldn't Parse Dependency List", error);
                }
            }
        }

        private void OnRepoBoxTextChanged(object sender, EventArgs e) {
            if (!finishedLoading) return;
            Log.Debug($"Updating github repo for {project} to '{repoBox.Input}'");
            project.repo = repoBox.Input;
            project.WriteManifestJson();
            ProjectManager.UpdateProject(project);
        }

        private void OnOpenRepoInBrowserClicked(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(project.repo)) {
                GuiUtils.ShowErrorMessage("No Repo Link Set", "You need to set a link to this Project's repo before clicking this.");
                return;
            }

            Log.Debug("Opening GitHub Repo link");
            Process.Start(new ProcessStartInfo() { UseShellExecute = true, FileName = project.repo });
        }

        private void OnThunderstoreLinkBoxTextChanged(object sender, EventArgs e) {
            if (!finishedLoading) return;
            Log.Debug($"Updating thunderstore link for {project} to '{thunderstoreLinkBox.Input}'");
            project.thunderstoreLink = thunderstoreLinkBox.Input;
            ProjectManager.UpdateProject(project);
        }

        private void OnOpenThunderstoreInBrowserClicked(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(project.thunderstoreLink)) {
                GuiUtils.ShowErrorMessage("No Thunderstore Link Set", "You need to set a link to this Project's Thunderstore page before clicking this.");
                return;
            }

            Log.Debug("Opening Thunderstore link");
            Process.Start(new ProcessStartInfo() { UseShellExecute = true, FileName = project.thunderstoreLink });
        }

        private void OnAddCreditsEntryClicked(object sender, EventArgs e) {
            string text = GuiUtils.GetStringFromUser("Enter Hyperlink Text", "Credits hyperlink text...");
            if (text == GetStringWindow.canceledInput) return;

            string url = GuiUtils.GetStringFromUser("Enter Hyperlink URL", "Credits hyperlink URL..");
            if (url == GetStringWindow.canceledInput) return;

            project.credits.Add($"[{text}]({url})");
            ProjectManager.UpdateProject(project);
            ShowCredits();
        }

        private void OnSourceBoxChangesConfirmed(object sender, EventArgs e) {
            if (Directory.Exists(sourceBox.Input)) {
                project.sourceFolder = sourceBox.Input;
                ProjectManager.UpdateProject(project);
                Log.Info($"Updated {project}.sourceFolder to {project.sourceFolder}");
            }
        }

        private void OnBrowseForSourceClicked(object sender, EventArgs e) {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                project.sourceFolder = dialog.SelectedPath;
                ProjectManager.UpdateProject(project);
                sourceBox.Input = project.sourceFolder;
                Log.Info($"Updated {project}.sourceFolder to {project.sourceFolder}");
            }
        }

        private void OnOpenSourceClicked(object sender, EventArgs e) {
            if (Directory.Exists(project.sourceFolder)) {
                Process.Start(project.sourceFolder);
            }
            else {
                GuiUtils.ShowErrorMessage("Source Folder Not Found", $"Could not find the folde '{project.sourceFolder}'. Please verify that it exists.");
            }
        }

        private void OnDescriptionBoxTextChanged(object sender, EventArgs e) {
            if (!finishedLoading) return;
            Log.Debug($"Updating description for {project}");
            project.description = descriptionBox.Input;
            ProjectManager.UpdateProject(project);
        }

        private void OnWrapTextToggled(object sender, EventArgs e) {
            if (!finishedLoading) return;
            Settings.userSettings.SetSetting(SettingNames.descriptionWrapText, wrapTextBox.IsChecked);

            if (wrapTextBox.IsChecked) {
                EnableTextWrapping();
            }
            else {
                DisableTextWrapping();
            }
        }

        // To Do List Events

        private void OnAddTicketClicked(object sender, EventArgs e) {
            Ticket ticket = TicketEditorWindow.CreateTicket();
            if(ticket == null) {
                Log.Debug("User canceled creating ticket");
                return;
            }

            project.tickets.Add(ticket);
            ProjectManager.UpdateProject(project);
            LoadToDoList();
        }

        private void OnTicketCompleteToggled(object sender, EventArgs e) {
            LoadToDoList();
        }

        // Mod Files Events

        private void OnPrepareUploadClicked(object sender, EventArgs e) {
            if(project.version == project.lastReleaseVersion) {
                GuiUtils.ShowWarningMessage($"Already Released {project.version}", "You've already released this version, you need to increase the version number.");
                return;
            }

            UploadWindow.UploadProject(ref project);
            ShowProject(project);
        }

        private async void OnFileSearchTimerTick(object sender, EventArgs e) {
            fileSearchTimer.Stop();
            await project.SearchForModFiles();
            LoadModFiles();
            fileSearchTimer.Start();
        }

        // Public Functions

        public void ShowProject(Project projectToShow) {
            Log.Debug($"Showing {projectToShow}");
            project = projectToShow;

            nameLabel.Content = project.name;
            majorBox.Input = project.version.major.ToString();
            minorBox.Input = project.version.minor.ToString();
            patchBox.Input = project.version.patch.ToString();

            Log.Debug($"Set Labels");

            releasedLabel.Visibility = project.isReleased ? Visibility.Visible : Visibility.Hidden;
            Log.Debug($"Set released visibility");

            tagLineBox.Input = project.tagline;
            dependenciesBox.Input = string.Join(", ", project.dependencies);
            repoBox.Input = project.repo;
            thunderstoreLinkBox.Input = project.thunderstoreLink;
            sourceBox.Input = project.sourceFolder;
            descriptionBox.Input = project.description;

            Log.Debug($"Set box inputs");

            wrapTextBox.IsChecked = Settings.userSettings.descriptionWrapText.value;
            if (wrapTextBox.IsChecked) {
                EnableTextWrapping();
            }
            else {
                DisableTextWrapping();
            }

            Log.Debug($"Set wrapping");
            
            ShowCredits();
            LoadToDoList();
            LoadModFiles();

            Log.Debug($"Populated lists");

            fileSearchTimer.Tick += OnFileSearchTimerTick;
            fileSearchTimer.Start();

            Log.Debug($"Started file search timer");

            finishedLoading = true;
        }

        // Private Functions

        private void ShowCredits() {
            List<string> entryTexts = new List<string>();
            foreach(string entry in project.credits) {
                int endIndex = entry.IndexOf(']');
                entryTexts.Add(entry.Substring(1, endIndex - 1));
            }

            creditsBox.Input = string.Join(", ", entryTexts);
        }

        private void LoadToDoList() {
            toDoList.Children.Clear();
            List<Ticket> tickets = project.GetSortedTickets();
            for(int i = 0; i < tickets.Count; i++) {
                Ticket ticket = tickets[i];
                TicketPanel panel = new TicketPanel(ref ticket) {
                    Margin = new Thickness(5, 5, 5, 0),
                    Height = 40
                };
                panel.CompletedToggled += OnTicketCompleteToggled;
                toDoList.Children.Add(panel);
            }
        }

        private void LoadModFiles() {
            project.hasIconFile = false;
            modFilesPanel.Children.Clear();
            List<string> allModFiles = GetFoldersAndFiles(project.GetFolder(), project.GetFolder());
            foreach (string modFile in allModFiles) {
                if (modFile.Contains("Source\\") || modFile.Contains("Resources\\")) continue;
                if (modFile.Contains("icon.png")) project.hasIconFile = true;

                modFilesPanel.Children.Add(new ModFilePanel(modFile, $"{project.GetFolder()}{modFile}") {
                    Margin = new Thickness(15 * GetIndentation(modFile), 5, 5, 0)
                });
            }
        }

        private List<string> GetFoldersAndFiles(string path, string root) {
            List<string> results = new List<string>();

            string[] folders = Directory.GetDirectories(path);
            foreach (string folder in folders) {
                results.Add(folder.Replace(root, ""));
                results.AddRange(GetFoldersAndFiles(folder, root));
            }

            string[] files = Directory.GetFiles(path);
            foreach (string file in files) {
                if (file.EndsWith(".zip")) continue;
                results.Add(file.Replace(root, ""));
            }

            return results;
        }

        private int GetIndentation(string fileName) {
            return fileName.Length - fileName.Replace("\\", "").Length - 1;
        }

        private void EnableTextWrapping() {
            descriptionBox.inputBox.TextWrapping = TextWrapping.Wrap;
            descriptionBox.inputBox.MaxWidth = 880;
            descriptionBox.inputScroller.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
        }

        private void DisableTextWrapping() {
            descriptionBox.inputBox.TextWrapping = TextWrapping.NoWrap;
            descriptionBox.inputBox.MaxWidth = 10000;
            descriptionBox.inputScroller.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
        }
    }
}
