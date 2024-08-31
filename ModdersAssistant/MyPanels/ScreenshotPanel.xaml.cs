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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ModdersAssistant.MyPanels
{
    /// <summary>
    /// Interaction logic for ScreenshotPanel.xaml
    /// </summary>
    public partial class ScreenshotPanel : UserControl
    {
        public ScreenshotPanel() {
            InitializeComponent();
        }

        public ScreenshotPanel(ref Screenshot screenshotToShow) {
            InitializeComponent();
            ShowScreenshot(ref screenshotToShow);
        }

        // Objects & Variables
        public Screenshot screenshot;

        // Custom Events
        public event EventHandler ScreenshotDeleted;

        // Events

        private void OnDeleteButtonClicked(object sender, EventArgs e) {
            ScreenshotDeleted?.Invoke(this, EventArgs.Empty);
            ((StackPanel)Parent).Children.Remove(this);
        }

        private void OnURLBoxTextChanged(object sender, EventArgs e) {
            screenshot.url = urlBox.Input;
        }

        // Private Functions

        private void ShowScreenshot(ref Screenshot screenshotToShow) {
            screenshot = screenshotToShow;
            nameLabel.Text = screenshot.name;
            urlBox.Input = screenshot.url;
        }
    }
}
