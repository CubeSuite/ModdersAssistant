using ModdersAssistant.MyClasses;
using ModdersAssistant.MyClasses.Globals;
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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ModdersAssistant.MyWindows
{
    /// <summary>
    /// Interaction logic for UploadWindow.xaml
    /// </summary>
    public partial class UploadWindow : Window
    {
        public UploadWindow() {
            Owner = Application.Current.MainWindow;
            InitializeComponent();
            Width = ProgramData.programWidth;
            Height = ProgramData.programHeight;
        }

        public UploadWindow(ref Project projectToUpload) {
            Owner = Application.Current.MainWindow;
            InitializeComponent();
            Width = ProgramData.programWidth;
            Height = ProgramData.programHeight;

            project = projectToUpload;
        }

        // Objects & Variables

        Project project;

        // Window Events

        private void OnCheckboxToggled(object sender, EventArgs e) {
            if (uploadImagesCheckbox.IsChecked) shader.Height = 240;
            if (enterLinksCheckbox.IsChecked) shader.Height = 200;
            if (checkManifestCheckbox.IsChecked) shader.Height = 160;
            if (checkReadmeCheckbox.IsChecked) {
                shader.Height = 120;
                if (project.hasIconFile) browseForIconCheckbox.IsChecked = true;
            }
            if (browseForIconCheckbox.IsChecked) shader.Height = 80;
            if (refreshFilesCheckbox.IsChecked) shader.Height = 40;
            if (zipFilesCheckbox.IsChecked) shader.Height = 0;
            if (uploadCheckbox.IsChecked) {
                confirmShader.Visibility = Visibility.Hidden;

                if (!string.IsNullOrEmpty(project.thunderstoreLink)) return;
                
                string link = GuiUtils.GetStringFromUser("Enter Thunderstore Link", "URL...");
                if (link == GetStringWindow.canceledInput) return;
                
                project.thunderstoreLink = link;
            }
        }

        private void OnConfirmUploadedClick(object sender, EventArgs e) {
            if(GuiUtils.GetUserConfirmation("Confirm As Released?", "Clicking 'Yes' will clear all completed tickets and add them to this project's changelog history.")) {
                if(GuiUtils.GetUserConfirmation("Generate Discord Message?", "Would you like to generate a Discord message to announce this release?")) {
                    string message = project.GenerateDiscordMessage();
                    Clipboard.SetText(message);
                    GuiUtils.ShowInfoMessage("Copied To Clipboard", $"The following message has been copied to your clipboard:\n\n{message}");
                }

                string changelog = MarkdownWriter.GetChangelog(project);
                project.changelogHistory = $"### {project.version}\n\n{changelog}\n\n{project.changelogHistory}";

                for(int i = 0; i < project.tickets.Count;) {
                    if (project.tickets[i].complete) {
                        project.tickets.RemoveAt(i);
                    }
                    else {
                        i++;
                    }
                }

                project.lastReleaseVersion = project.version;
                if(!string.IsNullOrEmpty(project.latestZipFile) && File.Exists(project.latestZipFile)) {
                    File.Delete(project.latestZipFile);
                }

                Close();
            }
        }

        private void OnCancelClicked(object sender, EventArgs e) {
            Close();
        }

        // Button Events

        private void OnUploadScreenshotsClicked(object sender, EventArgs e) {
            if (Directory.Exists(project.screenshotsFolder)) {
                Process.Start(project.screenshotsFolder);
            }

            Process.Start(new ProcessStartInfo() { UseShellExecute = true, FileName = "https://imgur.com/upload" });
        }

        private void OnEnterScreenshotLinksClicked(object sender, EventArgs e) {
            ScreenshotManagerWindow.ManageScreenshots(ref project);
        }

        private void OnCheckManifestClicked(object sender, EventArgs e) {
            string manifest = $"{project.modFilesFolder}\\manifest.json";
            if (File.Exists(manifest)) {
                File.Delete(manifest);
            }
            
            project.WriteManifestJson();
            Process.Start(manifest);
        }

        private void OnGenerateAndCheckReadMClicked(object sender, EventArgs e) {
            string readmePath = $"{project.modFilesFolder}\\README.md";
            if (File.Exists(readmePath)) {
                File.Delete(readmePath);
            }

            project.WriteReadMe();
            Process.Start(readmePath);
        }

        private void OnBrowseForIconClicked(object sender, EventArgs e) {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog() {
                Filter = "PNG Files (*.png)|*.png"
            };
            if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                project.hasIconFile = true;
                File.Copy(dialog.FileName, $"{project.modFilesFolder}\\icon.png");
                browseForIconCheckbox.IsChecked = true;
                OnCheckboxToggled(null, EventArgs.Empty);
            }
        }

        private async void OnRefreshFilesClicked(object sender, EventArgs e) {
            GuiUtils.ShowInfoMessage("Refreshing Mod Files", "Modder's Assistant will now refresh the mod files. When complete, the CheckBox will check itself.", "Continue");
            
            thisWindow.IsHitTestVisible = false;
            
            if(await project.RefreshFiles()) {
                refreshFilesCheckbox.IsChecked = true;
                OnCheckboxToggled(null, EventArgs.Empty);
            }

            thisWindow.IsHitTestVisible = true;
        }

        private void OnZipFilesClicked(object sender, EventArgs e) {
            if (project.ZipModFiles()) {
                zipFilesCheckbox.IsChecked = true;
                OnCheckboxToggled(null, EventArgs.Empty);
            }
        }

        private void OnUploadClicked(object sender, EventArgs e) {
            Process.Start(project.modFilesFolder);
            if (!string.IsNullOrEmpty(Settings.userSettings.thunderstoreUploadPage.value)) {
                Process.Start(new ProcessStartInfo() { UseShellExecute = true, FileName = Settings.userSettings.thunderstoreUploadPage.value });
            }
            else {
                GuiUtils.ShowWarningMessage("No Upload Page Set", $"Your '{SettingNames.thunderstoreUploadPage}' setting is blank,\nso you will need to open your upload page manually.");
            }
        }

        // Return Functions

        private string result;
        private string GetResult() { return result; }
        public static string UploadProject(ref Project project) {
            UploadWindow window = new UploadWindow(ref project);
            window.ShowDialog();
            return window.GetResult();
        }
    }
}
