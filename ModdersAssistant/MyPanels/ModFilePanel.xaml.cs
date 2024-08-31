using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ModdersAssistant.MyPanels
{
    /// <summary>
    /// Interaction logic for ModFilePanel.xaml
    /// </summary>
    public partial class ModFilePanel : UserControl
    {
        public ModFilePanel() {
            InitializeComponent();
        }

        public ModFilePanel(string fileName, string fullPath) {
            InitializeComponent();
            filePath = fullPath;
            ShowFile(fileName);
        }

        // Objects & Variables
        private string filePath;

        // Private Functions

        private void ShowFile(string fileName) {
            if (!fileName.Contains(".")) {
                svg.Source = new Uri($"{ProgramData.FilePaths.resourcesFolder}\\GUI\\Folder.svg");
                fileName = fileName.Split('\\').Last();
                fileNameLabel.Text = fileName;
                return;
            }

            if (fileName.EndsWith(".dll")) svg.Source = new Uri($"{ProgramData.FilePaths.resourcesFolder}\\GUI\\DLL.svg");
            else if (fileName.EndsWith(".json")) svg.Source = new Uri($"{ProgramData.FilePaths.resourcesFolder}\\GUI\\Json.svg");
            else if (fileName.EndsWith(".md")) svg.Source = new Uri($"{ProgramData.FilePaths.resourcesFolder}\\GUI\\Markdown.svg");
            else if (fileName.EndsWith(".png")) svg.Source = new Uri($"{ProgramData.FilePaths.resourcesFolder}\\GUI\\PNG.svg");
            else if (fileName.EndsWith(".cfg")) svg.Source = new Uri($"{ProgramData.FilePaths.resourcesFolder}\\ControlBox\\Settings.svg");
            else Log.Error($"Could not find icon for file: '{fileName}'");
            
            fileName = fileName.Split('\\').Last();
            fileNameLabel.Text = fileName;
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            if (filePath.EndsWith(".dll")) {
                GuiUtils.ShowErrorMessage("Cannot Open", $"{ProgramData.programName} cannot open a .dll file.");
                return;
            }

            Process.Start(filePath);
        }
    }
}
