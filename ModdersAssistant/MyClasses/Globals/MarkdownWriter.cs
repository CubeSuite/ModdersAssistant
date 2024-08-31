using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace ModdersAssistant.MyClasses.Globals
{
    public static class MarkdownWriter
    {
        // Objects & Variables
        private static Project project;

        // Public Functions

        public static string GetMarkdownForProject(Project projectToGenerateFor) {
            Log.Debug($"Generating Markdown for {project}");
            project = projectToGenerateFor;

            string markdown = $"# {project.name}\n\n";
            if (!string.IsNullOrEmpty(Settings.userSettings.bannerImageLink.value)) {
                markdown += $"![Banner Image]({Settings.userSettings.bannerImageLink.value})\n\n";
            }

            markdown += $"## Description\n\n{project.description}\n\n";

            List<string> screenshotLinks = GetScreenshotLinks();
            if(screenshotLinks.Count != 0) {
                markdown += $"## Screenshots\n\n{string.Join("\n", screenshotLinks)}\n\n";
            }

            if (project.hasConfigFile) markdown += GetConfigOptionsTable();

            markdown += $"## Installation\n\n{StringUtils.GetInstallationTemplate()}\n\n";

            if(project.tickets.Count != 0) {
                string upcomingSection = GetUpcomingFeaturesSection();
                if (!string.IsNullOrEmpty(upcomingSection)) {
                    markdown += $"## Upcoming Features & Changes\n\n{upcomingSection}\n";
                }

                string knownIssues = GetKnownIssuesSection();
                if (!string.IsNullOrEmpty(knownIssues)) {
                    markdown += $"## Known Issues\n\n{knownIssues}\n";
                }

                string changelog = GetChangelog();
                if (!string.IsNullOrEmpty(changelog)) {
                    markdown += $"## Changelog\n\n### {project.version}\n\n{changelog}\n";
                }
            }

            if (!string.IsNullOrEmpty(project.changelogHistory)) {
                markdown += $"{project.changelogHistory}\n\n";
            }

            markdown += $"## Credits\n\n{string.Join("\n", project.credits)}\n\n";
            markdown += $"## Disclaimer\n\n{StringUtils.GetDisclaimerTemplate()}\n\n";

            return markdown;
        }

        public static string GetChangelog(Project targetProject) {
            project = targetProject;
            return GetChangelog();
        }

        public static string GetChangelog() {
            string section = "";
            List<Ticket> tickets = project.tickets.Where(ticket => ticket.complete).ToList();
            foreach (Ticket ticket in tickets) {
                string prefix = "";
                switch (ticket.type) {
                    case TicketType.NewFeature: prefix = "Added: "; break;
                    case TicketType.Change: prefix = "Changed: "; break;
                    case TicketType.Bug: prefix = "Fixed: "; break;
                }

                section += $"- {prefix}{ticket.title}\n  - {ticket.description}\n";
            }

            // ToDo: Add Changelog History

            return section;
        }

        // Private Functions

        private static List<string> GetScreenshotLinks() {
            List<string> screenshotLinks = new List<string>();
            foreach (Screenshot screenshot in project.screenshots) {
                screenshotLinks.Add($"![{screenshot.name}]({screenshot.url})");
            }

            return screenshotLinks;
        }

        private static string GetConfigOptionsTable() {
            string configFile = Directory.GetFiles($"{project.modFilesFolder}/config").First();
            ModConfig config = ModConfig.FromFile(configFile);
            return $"## Config Options\n\n{ConfigTableGenerator.GenerateTable(config)}\n\n";
        }

        private static string GetUpcomingFeaturesSection() {
            string section = "";
            List<Ticket> tickets = project.tickets.Where(ticket => !ticket.complete && ticket.type != TicketType.Bug).ToList();
            foreach(Ticket ticket in tickets) {
                section += $"- {ticket.title}\n  - {ticket.description}\n";
            }

            return section;
        }

        private static string GetKnownIssuesSection() {
            string section = "";
            List<Ticket> tickets = project.tickets.Where(ticket => !ticket.complete && ticket.type == TicketType.Bug).ToList();
            foreach (Ticket ticket in tickets) {
                section += $"- {ticket.title}\n  - {ticket.description}\n";
            }

            return section;
        }
    }

    public static class ConfigTableGenerator 
    {
        // Objects & Variables
        private static ModConfig config;
        private static int numColumns;
        private static int numRows;
        private static string[,] tableValues;

        // Public Functions

        public static string GenerateTable(ModConfig configToGenerateFor) {
            config = configToGenerateFor;
            numRows = config.options.Count;
            numColumns = GetNumColumns();

            tableValues = new string[numRows + 2, numColumns];
            SetColumnHeaders();
            SetCellData();
            PadCells();

            return FormTable();
        }

        // Private Functions

        private static int GetNumColumns() {
            int numColumns = 4;
            foreach (ConfigOption option in config.options) {
                if (option is IntConfigOption intOption) {
                    if (intOption.min != int.MinValue || intOption.max != int.MaxValue) {
                        numColumns = 5;
                        break;
                    }
                }
                else if (option is FloatConfigOption floatOption) {
                    if (floatOption.min != float.MinValue || floatOption.max != float.MaxValue) {
                        numColumns = 5;
                        break;
                    }
                }
                else if (option is DoubleConfigOption doubleOption) {
                    if (doubleOption.min != float.MinValue || doubleOption.max != float.MaxValue) {
                        numColumns = 5;
                        break;
                    }
                }
            }
            
            return numColumns;
        }

        private static void SetColumnHeaders() {
            tableValues[0, 0] = "Name";
            tableValues[0, 1] = "Type";
            tableValues[0, 2] = "Description";
            tableValues[0, 3] = "Default Value";
            if (numColumns == 5) tableValues[0, 4] = "Range";
        }

        private static void SetCellData() {
            for (int row = 2; row < numRows + 2; row++) {
                ConfigOption option = config.options[row - 2];
                tableValues[row, 0] = option.name;
                tableValues[row, 2] = option.GetDescription();

                if (option is StringConfigOption stringOption) {
                    tableValues[row, 1] = "Text";
                    tableValues[row, 3] = stringOption.defaultValue;
                    if (numColumns == 5) tableValues[row, 4] = "";
                }
                else if (option is KeyboardShortcutConfigOption shortcutOption) {
                    tableValues[row, 1] = "Shortcut";
                    tableValues[row, 3] = shortcutOption.defaultValue;
                    if (numColumns == 5) tableValues[row, 4] = "";
                }
                else if (option is BooleanConfigOption boolOption) {
                    tableValues[row, 1] = "Toggle";
                    tableValues[row, 3] = boolOption.defaultValue ? "True" : "False";
                    if (numColumns == 5) tableValues[row, 4] = "";
                }
                else if (option is IntConfigOption intOption) {
                    tableValues[row, 1] = "Integer";
                    tableValues[row, 3] = intOption.defaultValue.ToString();
                    if (numColumns == 5) tableValues[row, 4] = $"{intOption.min} -> {intOption.max}";
                }
                else if (option is FloatConfigOption floatOption) {
                    tableValues[row, 1] = "Decimal";
                    tableValues[row, 3] = floatOption.defaultValue.ToString();
                    if (numColumns == 5) tableValues[row, 4] = $"{floatOption.min} -> {floatOption.max}";
                }
                else if (option is DoubleConfigOption doubleOption) {
                    tableValues[row, 1] = "Decimal";
                    tableValues[row, 3] = doubleOption.defaultValue.ToString();
                    if (numColumns == 5) tableValues[row, 4] = $"{doubleOption.min} -> {doubleOption.max}";
                }
            }
        }

        private static void PadCells() {
            int maxNameWidth = 0;
            int maxTypeWidth = 0;
            int maxDescriptionWidth = 0;
            int maxDefaultWidth = 0;
            int maxRangeWidth = 0;

            for (int row = 0; row < numRows + 2; row++) {
                if (row == 1) continue; // Skip row that separates headers and values
                if (tableValues[row, 0].Length > maxNameWidth) maxNameWidth = tableValues[row, 0].Length;
                if (tableValues[row, 1].Length > maxTypeWidth) maxTypeWidth = tableValues[row, 1].Length;
                if (tableValues[row, 2].Length > maxDescriptionWidth) maxDescriptionWidth = tableValues[row, 2].Length;
                if (tableValues[row, 3].Length > maxDefaultWidth) maxDefaultWidth = tableValues[row, 3].Length;
                if (numColumns == 5 && tableValues[row, 4].Length > maxRangeWidth) maxRangeWidth = tableValues[row, 4].Length;
            }

            for (int row = 0; row < numRows + 2; row++) {
                if (row == 1) continue; // Skip row that separates headers and values
                tableValues[row, 0] = tableValues[row, 0].PadRight(maxNameWidth);
                tableValues[row, 1] = PadCentre(tableValues[row, 1], maxTypeWidth);
                tableValues[row, 2] = tableValues[row, 2].PadRight(maxDescriptionWidth);
                tableValues[row, 3] = PadCentre(tableValues[row, 3], maxDefaultWidth);
                if (numColumns == 5) tableValues[row, 4] = PadCentre(tableValues[row, 4], maxRangeWidth);
            }

            tableValues[1, 0] = "".PadRight(maxNameWidth, '-');
            tableValues[1, 1] = ":".PadRight(maxTypeWidth - 1, '-') + ':';
            tableValues[1, 2] = "".PadRight(maxDescriptionWidth, '-');
            tableValues[1, 3] = ":".PadRight(maxDefaultWidth - 1, '-') + ':';
            if (numColumns == 5) tableValues[1, 4] = ":".PadRight(maxRangeWidth - 1, '-') + ':';
        }

        private static string FormTable() {
            string table = "";
            for (int row = 0; row < numRows + 2; row++) {
                table += "|" + tableValues[row, 0];
                table += "|" + tableValues[row, 1];
                table += "|" + tableValues[row, 2];
                table += "|" + tableValues[row, 3];
                if (numColumns == 5) table += "|" + tableValues[row, 4];
                table += "|\n";
            }

            return table;
        }

        private static string PadCentre(string input, int length) {
            bool padLeft = true;
            while (input.Length < length) {
                if (padLeft) input = ' ' + input;
                else input += ' ';
                padLeft = !padLeft;
            }

            return input;
        }
    }
}
