using ModdersAssistant.MyControls;
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
using ModdersAssistant.MyWindows.GetWindows;
using System.IO;

namespace ModdersAssistant.MyPanels
{
    /// <summary>
    /// Interaction logic for ProjectsListPanel.xaml
    /// </summary>
    public partial class ProjectsListPanel : UserControl
    {
        public ProjectsListPanel() {
            InitializeComponent();
        }

        // Objects & Variables

        public int clickedProjectID;

        // Custom Events

        public event EventHandler ProjectClicked;

        // Events

        private void OnPanelLoaded(object sender, RoutedEventArgs e) {
            LoadAllProjects();
        }

        private void OnAddProjectClicked(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(Settings.userSettings.projectsFolder.value)) {
                GuiUtils.ShowErrorMessage("No Projects Folder Set", "Head to the settings and choose a folder to store Project folders in.");
                return;
            }

            if (!Directory.Exists(Settings.userSettings.projectsFolder.value)) {
                GuiUtils.ShowErrorMessage("Invalid Projects Folder", "Your chosen Projects folder does not exist, please create it or choose another");
                return;
            }
            
            string newProjectName = GuiUtils.GetStringFromUser("Enter Project Name", "Name...");
            if (newProjectName == GetStringWindow.canceledInput) {
                Log.Debug("User canceled adding new Project");
                return;
            }

            if (string.IsNullOrEmpty(newProjectName)) {
                GuiUtils.ShowErrorMessage("Invalid Name", "Your project's name cannot be blank.");
                return;
            }

            if (!ProjectManager.IsNameUnique(newProjectName)) {
                GuiUtils.ShowErrorMessage("Invalid Name", "This name is not unique.");
                return;
            }

            Log.Debug($"Creating Project '{newProjectName}'");
            Project project = new Project() { name = newProjectName };
            project.CreateFolders();
            project.WriteManifestJson();
            project.WriteReadMePlaceHolder();

            clickedProjectID = ProjectManager.AddProject(project);
            searchBar.Input = ""; // This triggers list refresh
            ProjectClicked?.Invoke(this, EventArgs.Empty);
        }

        private void OnSearchBarTextChanged(object sender, EventArgs e) {
            string searchTerm = searchBar.Input;
            LoadProjects(ProjectManager.SearchForProjects(searchTerm));
        }

        private void OnProjectButtonClicked(object sender, EventArgs e) {
            MyButton clickedButton = sender as MyButton;
            clickedProjectID = clickedButton.projectID;
            ProjectClicked?.Invoke(this, EventArgs.Empty);
        }

        // Public Functions

        public void LoadAllProjects() {
            LoadProjects(ProjectManager.GetAllProjects().OrderBy(project => project.name).ToList());
        }

        public void RefreshAddButton() {
            addButton.Source = null;
            addButton.Source = "GUI/Add";
        }

        // Private Functions

        private void LoadProjects(List<Project> projects) {
            projectsPanel.Children.Clear();
            foreach(Project project in projects) {
                AddButtonForProject(project);
            }
        }

        private void AddButtonForProject(Project project) {
            MyButton button = new MyButton() {
                projectID = project.id,
                ButtonText = project.name,
                Margin = new Thickness(5, 5, 5, 0)
            };
            button.LeftClicked += OnProjectButtonClicked;
            projectsPanel.Children.Add(button);
        }
    }
}
