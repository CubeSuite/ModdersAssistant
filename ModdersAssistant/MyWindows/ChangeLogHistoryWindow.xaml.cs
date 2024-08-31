using ModdersAssistant.MyClasses;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for ChangeLogHistoryWindow.xaml
    /// </summary>
    public partial class ChangeLogHistoryWindow : Window
    {
        public ChangeLogHistoryWindow(ref Project projectToEdit) {
            Owner = Application.Current.MainWindow;
            InitializeComponent();
            Width = ProgramData.programWidth;
            Height = ProgramData.programHeight;
            
            project = projectToEdit;
            inputBox.Input = project.changelogHistory;
        }

        // Objects & Variables
        Project project;

        // Events

        private void OnCloseClicked(object sender, EventArgs e) {
            Close();
        }

        private void OnInputBoxTextChanged(object sender, EventArgs e) {
            project.changelogHistory = inputBox.Input;
        }

        // Return Function

        private string result;
        private string GetResult() { return result; }
        public static string EditChangelogHistory(ref Project projectToEdit) {
            ChangeLogHistoryWindow window = new ChangeLogHistoryWindow(ref projectToEdit);
            window.ShowDialog();
            return window.GetResult();
        }
    }
}
