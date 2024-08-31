using ModdersAssistant.MyClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModdersAssistant
{
    public static class ProjectManager
    {
        // Objects & Variables

        private static Dictionary<int, Project> projects = new Dictionary<int, Project>();

        // Public Functions

        public static int AddProject(Project project) {
            project.id = getNewID();
            projects.Add(project.id, project);
            Log.Debug($"Added Project with id '{project.id}'");
            
            return project.id;
        }

        public static void UpdateProject(Project project) {
            if (DoesProjectExist(project)) {
                projects[project.id] = project;
                Log.Debug($"Updated Project with id '{project.id}'");
            }
            else {
                Log.Error($"Could not update unknown Project with id '{project.id}'");
            }
        }

        public static Project GetProject(int id) {
            if (DoesProjectExist(id)) {
                return projects[id];
            }
            else {
                string error = $"Cannot get Project #{id}, does not exist";
                Log.Error(error);
                DebugUtils.CrashIfDebug(error);
                return null;
            }
        }

        public static List<Project> GetAllProjects() {
            return projects.Values.OrderBy(project => project.name).ToList();
        }

        public static List<Project> SearchForProjects(string searchTerm) {
            return GetAllProjects().Where(project => project.name.ToLower().Contains(searchTerm)).ToList();
        }

        public static int GetProjectsCount() {
            return projects.Count;
        }

        public static bool DoesProjectExist(int id) {
            return projects.ContainsKey(id);
        }

        public static bool IsNameUnique(string name) {
            return projects.Values.Where(project => project.name.ToLower() == name.ToLower()).Count() == 0;
        }

        // Private Functions

        private static int getNewID() {
            if (GetProjectsCount() == 0) return 0;
            else return projects.Keys.Max() + 1;
        }

        // Data Functions

        public static void Save() {
            string json = JsonConvert.SerializeObject(projects.Values.ToList());
            File.WriteAllText(ProgramData.FilePaths.projectsSaveFile, json);
            Log.Info("Saved Projects");
        }

        public static void Load() {
            if (!File.Exists(ProgramData.FilePaths.projectsSaveFile)) return;

            string json = File.ReadAllText(ProgramData.FilePaths.projectsSaveFile);
            List<Project> projectsFromFile = JsonConvert.DeserializeObject<List<Project>>(json);
            foreach (Project project in projectsFromFile) {
                projects[project.id] = project;
            }

            Log.Info("Loaded Projects");
        }

        #region Overloads

        // Public Functions

        public static bool DoesProjectExist(Project project) {
            return DoesProjectExist(project.id);
        }

        #endregion
    }
}
