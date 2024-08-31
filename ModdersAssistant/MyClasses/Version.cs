using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModdersAssistant.MyClasses
{
    public struct Version {
        // Objects & Variables
        public int major;
        public int minor;
        public int patch;

        // Constructors

        public static Version GetDefault(){
            return new Version() {
                major = 1,
                minor = 0,
                patch = 0
            };
        }

        public Version(int _major, int _minor, int _patch) {
            major = _major;
            minor = _minor;
            patch = _patch; 
        }

        // Public Functions

        public override string ToString() {
            return $"V{major}.{minor}.{patch}";
        }

        public static bool operator ==(Version v1, Version v2) {
            return v1.major == v2.major && v1.minor == v2.minor && v1.patch == v2.patch;
        }

        public static bool operator !=(Version v1, Version v2) {
            return v1.major != v2.major || v1.minor != v2.minor || v1.patch != v2.patch;
        }
    }
}
