using ModdersAssistant.MyClasses;
using ModdersAssistant.MyPanels;
using ModdersAssistant.MyWindows;
using SharpVectors.Dom.Css;
using SharpVectors.Scripting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ModdersAssistant
{
    public partial class MainWindow : Window
    {
        public MainWindow() {
            InitializeComponent();
        }

        // Objects & Variables
        public static MainWindow current => (MainWindow)Application.Current.MainWindow;
        private static DispatcherTimer autoSaveTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(15) };

        // Program Events

        // ToDo: Browse for icon.png
        // ToDo: Only link file in screenshot if .png || .jpg || .jpeg

        private void OnProgramLoaded(object sender, RoutedEventArgs e) {
            titleLabel.Content = $"{ProgramData.programName} - V{ProgramData.versionText}";
            Width = ProgramData.programWidth;
            Height = ProgramData.programHeight;

            ProgramData.FilePaths.CreateFolderStructure();
            ProgramData.FilePaths.GenerateResources();
            projectsList.RefreshAddButton();

            Log.InitialiseLog();
            Log.Info($"Log initialised at {DateTime.Now}");
            if (ProgramData.isDebugBuild) Log.Warning("This is a debug build");

            LoadData();
            Log.Info("Data Loaded");

            InitialiseAutoSaveTimer();
        }

        private async void OnProgramClosing(object sender, System.ComponentModel.CancelEventArgs e) {
            Settings.userSettings.SetSetting(SettingNames.isFirstTimeLaunch, false);
            SaveData();
            await BackupManager.AutoBackup();
            Log.Info("Data saved on program close");
        }

        private void OnAutoSaveTimerTick(object sender, EventArgs e) {
            SaveData();
        }

        // GUI Events

        private void OnProjectClicked(object sender, EventArgs e) {
            if(projectPanelBorder.Child is ProjectPanel currentPanel) {
                currentPanel.fileSearchTimer.Stop();
            }

            Project project = ProjectManager.GetProject(projectsList.clickedProjectID);
            ProjectPanel panel = new ProjectPanel(project);
            panel.ProjectNameEdited += OnProjectNameEdited;
            projectPanelBorder.Child = panel;
        }

        private void OnProjectNameEdited(object sender, EventArgs e) {
            projectsList.LoadAllProjects();
        }

        // Private Functions

        private void InitialiseAutoSaveTimer() {
            autoSaveTimer.Tick += OnAutoSaveTimerTick;
            autoSaveTimer.Start();
        }

        private void SaveData() {
            if (ProgramData.safeToSave) {
                try {
                    Settings.Save();
                    ProjectManager.Save();
                    Log.Debug("All Data saved");
                }
                catch (Exception error) {
                    Log.Error($"Error occurred while trying to save data: ");
                    Log.Error(error.Message);
                    Log.Error(error.StackTrace);
                }
            }
            else {
                Log.Warning("Save skipped");
            }
        }

        private void LoadData() {
            Settings.Load();
            ProjectManager.Load();
            Log.Debug($"All Data Loaded");
            ProgramData.safeToSave = true;
        }
    }
}
