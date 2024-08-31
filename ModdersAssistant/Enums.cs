using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;

namespace ModdersAssistant
{
    #region TicketType

    public enum TicketType
    {
        NewFeature,
        Change,
        Bug,
        Null,
    }

    public static partial class StringUtils
    {
        public static TicketType GetTicketTypeFromName(string name) {
            switch (name) {
                case "New Feature": return TicketType.NewFeature;
                case "Change": return TicketType.Change;
                case "Bug": return TicketType.Bug;

                default:
                    Log.Error($"No member of TicketType has name '{name}'");
                    return TicketType.Null;
            }
        }

        public static string GetTicketTypeName(TicketType myEnum) {
            switch (myEnum) {
                case TicketType.NewFeature: return "New Feature";
                case TicketType.Change: return "Change";
                case TicketType.Bug: return "Bug";
                case TicketType.Null: return "Null";
                default:
                    string defaultName = Enum.GetName(typeof(TicketType), myEnum);
                    Log.Error($"Name not set for TicketType member: {defaultName}");
                    return defaultName;
            }
        }

        public static List<string> GetAllTicketTypeNames() {
            List<string> allNames = new List<string>();
            foreach (TicketType myEnum in Enum.GetValues(typeof(TicketType))) {
                string name = GetTicketTypeName(myEnum);
                if (name != "Null") allNames.Add(name);
            }

            return allNames;
        }

        public static string GetAllTicketTypeNamesForCombo() {
            return string.Join("|", GetAllTicketTypeNames());
        }
    }

    #endregion
}
