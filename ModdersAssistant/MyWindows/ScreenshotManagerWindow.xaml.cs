using ModdersAssistant.MyClasses;
using ModdersAssistant.MyPanels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace ModdersAssistant.MyWindows
{
    /// <summary>
    /// Interaction logic for ScreenshotManagerWindow.xaml
    /// </summary>
    public partial class ScreenshotManagerWindow : Window
    {
        public ScreenshotManagerWindow() {
            Owner = Application.Current.MainWindow;
            InitializeComponent();
            Width = ProgramData.programWidth;
            Height = ProgramData.programHeight;
        }

        public ScreenshotManagerWindow(ref Project parentProject) {
            Owner = Application.Current.MainWindow;
            InitializeComponent();
            Width = ProgramData.programWidth;
            Height = ProgramData.programHeight;

            project = parentProject;
            ShowScreenshots();
        }

        // Objects & Variables
        public Project project;

        // Events

        private void OnScreenshotDeleted(object sender, EventArgs e) {
            ScreenshotPanel panel = sender as ScreenshotPanel;
            project.screenshots.Remove(panel.screenshot);
            Log.Info($"Removed screenshot '{panel.screenshot.name}' from {project}");
        }

        private void OnCloseClicked(object sender, EventArgs e) {
            foreach(Screenshot screenshot in project.screenshots) {
                if(screenshot.url.Contains("imgur") && !screenshot.url.Contains("i.imgur")) {
                    GuiUtils.ShowWarningMessage("Invalid Image Link", "This image will not render on Thunderstore.\n" +
                                                "Open the link, right click the image, select 'Open image in new tab', copy the url of the new tab.");
                    screenshot.url = "";
                    ShowScreenshots();
                    return;
                }
            }

            Close();
        }

        // Private Functions

        private void ShowScreenshots() {
            Log.Debug($"Showing screenshots for {project}");
            screenshotsPanel.Children.Clear();
            for(int i = 0; i < project.screenshots.Count; i++) {
                Screenshot screenshot = project.screenshots[i];
                ScreenshotPanel panel = new ScreenshotPanel(ref screenshot) { Height = 70, Margin = new Thickness(5, 5, 5, 0) };
                panel.ScreenshotDeleted += OnScreenshotDeleted;
                screenshotsPanel.Children.Add(panel);
            }
        }

        // Return Functions

        private string result;
        private string GetResult() { return result; }
        public static string ManageScreenshots(ref Project parentProject) {
            ScreenshotManagerWindow window = new ScreenshotManagerWindow(ref parentProject);
            window.ShowDialog();
            return window.GetResult();
        }
    }
}
