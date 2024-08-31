using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModdersAssistant.MyClasses.Globals
{
    public static class DebugTools
    {
        public static void DebugCrash(string message) {
            if (ProgramData.isDebugBuild) {
                throw new Exception(message);
            }
        }
    }
}
