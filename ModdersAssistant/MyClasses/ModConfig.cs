using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ModdersAssistant.MyClasses
{
    public class ModConfig
    {
        // Objects & Variables
        public static ModConfig activeConfig;

        public string filePath;
        public List<ConfigOption> options = new List<ConfigOption>();

        // Public Functions

        public static ModConfig FromFile(string filename) {
            Log.Debug($"Reading config file from '{filename}'");
            ModConfig config = new ModConfig() { filePath = filename };
            string[] lines = File.ReadAllLines(filename);

            bool startedConfigFile = false;
            string latestCategory = "";
            for (int i = 0; i < lines.Length; i++) {
                string thisLine = lines[i];
                if (thisLine.StartsWith("[")) {
                    startedConfigFile = true;
                    latestCategory = thisLine.Replace("[", "").Replace("]", "");
                    Log.Debug($"Found category '{latestCategory}'");
                }

                if (thisLine.StartsWith("#") && startedConfigFile) {
                    int offset = thisLine.StartsWith("##") ? 1 : 0;
                    string optionType = lines[i + offset].Split(new string[] { ": " }, StringSplitOptions.None).Last();
                    List<string> optionLines = new List<string>() { };
                    for (int j = i; j < lines.Count(); j++) {
                        if (string.IsNullOrEmpty(lines[j]) || string.IsNullOrWhiteSpace(lines[j]) || j == lines.Count() - 1) {
                            for (int k = i; k < j; k++) {
                                optionLines.Add(lines[k]);
                                i = k;
                            }

                            break;
                        }
                    }

                    switch (optionType) {
                        case "String": config.options.Add(new StringConfigOption(optionLines) { category = latestCategory }); break;
                        case ConfigOptionTypes.keyboardShortcutOption: config.options.Add(new KeyboardShortcutConfigOption(optionLines) { category = latestCategory }); break;
                        case "Int32": config.options.Add(new IntConfigOption(optionLines) { category = latestCategory }); break;
                        case "Single": config.options.Add(new FloatConfigOption(optionLines) { category = latestCategory }); break;
                        case "Double": config.options.Add(new DoubleConfigOption(optionLines) { category = latestCategory }); break;
                        case "Boolean": config.options.Add(new BooleanConfigOption(optionLines) { category = latestCategory }); break;
                        default:
                            Log.Error($"Cannot add config option for unknown type: '{optionType}'");
                            break;
                    }
                }
            }

            return config;
        }
    }

    public class ConfigOption
    {
        public string name;
        public string description;
        public string category;
        public string optionType;

        public string GetDescription() {
            if (!string.IsNullOrEmpty(description)) return description;
            else return "No description available.";
        }
    }

    public static class ConfigOptionTypes
    {
        public const string stringOption = "String";
        public const string keyboardShortcutOption = "KeyboardShortcut";
        public const string intOption = "Int32";
        public const string floatOption = "Single";
        public const string doubleOption = "Double";
        public const string boolOption = "Boolean";
    }

    public class StringConfigOption : ConfigOption
    {
        public string value;
        public string defaultValue;

        public StringConfigOption() { optionType = ConfigOptionTypes.stringOption; }
        public StringConfigOption(List<string> fileLines) {
            optionType = "String";
            foreach (string line in fileLines) {
                if (line.StartsWith("## ")) {
                    description = line.Replace("## ", "");
                }
                else if (line.StartsWith("# Default")) {
                    defaultValue = line.Split(new string[] { ": " }, StringSplitOptions.None).Last();
                }
                else if (!line.StartsWith("# Setting type")) {
                    name = line.Split(new string[] { " = " }, StringSplitOptions.None).First();
                    value = line.Split(new string[] { " = " }, StringSplitOptions.None).Last();
                }
            }
        }
    }

    public class KeyboardShortcutConfigOption : ConfigOption
    {
        public string value;
        public string defaultValue;

        public KeyboardShortcutConfigOption() { optionType = ConfigOptionTypes.keyboardShortcutOption; }
        public KeyboardShortcutConfigOption(List<string> fileLines) {
            optionType = ConfigOptionTypes.keyboardShortcutOption;
            foreach (string line in fileLines) {
                if (line.StartsWith("## ")) {
                    description = line.Replace("## ", "");
                }
                else if (line.StartsWith("# Default")) {
                    defaultValue = line.Split(new string[] { ": " }, StringSplitOptions.None).Last();
                }
                else if (!line.StartsWith("# Setting type")) {
                    name = line.Split(new string[] { " = " }, StringSplitOptions.None).First();
                    value = line.Split(new string[] { " = " }, StringSplitOptions.None).Last();
                }
            }
        }
    }

    public class IntConfigOption : ConfigOption
    {
        public int value;
        public int defaultValue;
        public int min = int.MinValue;
        public int max = int.MaxValue;

        public IntConfigOption(List<string> fileLines) {
            optionType = ConfigOptionTypes.intOption;
            foreach (string line in fileLines) {
                if (line.StartsWith("## ")) {
                    description = line.Replace("## ", "");
                }
                else if (line.StartsWith("# Default")) {
                    defaultValue = int.Parse(line.Split(new string[] { ": " }, StringSplitOptions.None).Last());
                }
                else if (line.StartsWith("# Accept")) {
                    ExtractMinMaxValues(line, out min, out max);
                }
                else if (!line.StartsWith("# Setting type")) {
                    name = line.Split(new string[] { " = " }, StringSplitOptions.None).First();
                    value = int.Parse(line.Split(new string[] { " = " }, StringSplitOptions.None).Last());
                }
            }
        }

        bool ExtractMinMaxValues(string input, out int minValue, out int maxValue) {
            string pattern = @"From (-?\d+) to (-?\d+)";
            Match match = Regex.Match(input, pattern);

            if (match.Success) {
                if (int.TryParse(match.Groups[1].Value, out int min) && int.TryParse(match.Groups[2].Value, out int max)) {
                    if (min <= max) {
                        minValue = min;
                        maxValue = max;
                        return true;
                    }
                }
            }

            minValue = int.MinValue;
            maxValue = int.MaxValue;
            return false;
        }
    }

    public class FloatConfigOption : ConfigOption
    {
        public float value;
        public float defaultValue;
        public float min = float.MinValue;
        public float max = float.MaxValue;

        public FloatConfigOption(List<string> fileLines) {
            optionType = ConfigOptionTypes.floatOption;
            foreach (string line in fileLines) {
                if (line.StartsWith("## ")) {
                    description = line.Replace("## ", "");
                }
                else if (line.StartsWith("# Default")) {
                    defaultValue = float.Parse(line.Split(new string[] { ": " }, StringSplitOptions.None).Last());
                }
                else if (line.StartsWith("# Accept")) {
                    ExtractMinMaxValues(line, out min, out max);
                }
                else if (!line.StartsWith("# Setting type")) {
                    name = line.Split(new string[] { " = " }, StringSplitOptions.None).First();
                    value = float.Parse(line.Split(new string[] { " = " }, StringSplitOptions.None).Last());
                }
            }
        }

        bool ExtractMinMaxValues(string input, out float minValue, out float maxValue) {
            string pattern = @"From (-?\d+(\.\d+)?) to (-?\d+(\.\d+)?)";
            Match match = Regex.Match(input, pattern);

            if (match.Success) {
                if (float.TryParse(match.Groups[1].Value, out float min) && float.TryParse(match.Groups[2].Value, out float max)) {
                    if (min <= max) {
                        minValue = min;
                        maxValue = max;
                        return true;
                    }
                }
            }

            minValue = float.MinValue;
            maxValue = float.MaxValue;
            return false;
        }
    }

    public class DoubleConfigOption : ConfigOption
    {
        public double value;
        public double defaultValue;
        public double min = double.MinValue;
        public double max = double.MaxValue;

        public DoubleConfigOption(List<string> fileLines) {
            optionType = ConfigOptionTypes.doubleOption;
            foreach (string line in fileLines) {
                if (line.StartsWith("## ")) {
                    description = line.Replace("## ", "");
                }
                else if (line.StartsWith("# Default")) {
                    defaultValue = double.Parse(line.Split(new string[] { ": " }, StringSplitOptions.None).Last());
                }
                else if (line.StartsWith("# Accept")) {
                    ExtractMinMaxValues(line, out min, out max);
                }
                else if (!line.StartsWith("# Setting type")) {
                    name = line.Split(new string[] { " = " }, StringSplitOptions.None).First();
                    value = double.Parse(line.Split(new string[] { " = " }, StringSplitOptions.None).Last());
                }
            }
        }

        bool ExtractMinMaxValues(string input, out double minValue, out double maxValue) {
            string pattern = @"From (-?\d+(\.\d+)?) to (-?\d+(\.\d+)?)";
            Match match = Regex.Match(input, pattern);

            if (match.Success) {
                if (double.TryParse(match.Groups[1].Value, out double min) && double.TryParse(match.Groups[2].Value, out double max)) {
                    if (min <= max) {
                        minValue = min;
                        maxValue = max;
                        return true;
                    }
                }
            }

            minValue = double.MinValue;
            maxValue = double.MaxValue;
            return false;
        }
    }

    public class BooleanConfigOption : ConfigOption
    {
        public bool value;
        public bool defaultValue;

        public BooleanConfigOption() { optionType = ConfigOptionTypes.boolOption; }
        public BooleanConfigOption(List<string> fileLines) {
            optionType = ConfigOptionTypes.boolOption;
            foreach (string line in fileLines) {
                if (line.StartsWith("## ")) {
                    description = line.Replace("## ", "");
                }
                else if (line.StartsWith("# Default")) {
                    defaultValue = line.Split(new string[] { ": " }, StringSplitOptions.None).Last() == "true";
                }
                else if (!line.StartsWith("# Setting type")) {
                    name = line.Split(new string[] { " = " }, StringSplitOptions.None).First();
                    value = line.Split(new string[] { " = " }, StringSplitOptions.None).Last() == "true";
                }
            }
        }
    }
}
