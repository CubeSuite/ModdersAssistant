using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ModdersAssistant.MyClasses
{
    public class Setting
    {
        // Objects & Variables
        [JsonIgnore] public readonly string type = "None";
        [JsonIgnore] public string name;
        [JsonIgnore] public string description;
        [JsonIgnore] public string category;
        [JsonIgnore] public bool isHidden = false;

        // Public Functions
        public virtual void RestoreDefault() { }

        // Constructors
        public Setting() { }
        public Setting(string _type) {
            type = _type;
        }
    }

    public class StringSetting : Setting
    {
        // Objects & Variables
        private string _value;
        public string value {
            get => _value;
            set {
                _value = value;
                OnValueChanged(value);
            }
        }

        [JsonIgnore] public string defaultValue;
        [JsonIgnore] public Action<string> OnValueChanged = (string newValue) => { };

        // Public Functions

        public override void RestoreDefault() {
            value = defaultValue;
        }

        // Constructors

        public StringSetting() : base("String") { }
        public StringSetting(string _default) : base("String") {
            value = _default;
            defaultValue = _default;
        }
    }

    public class ComboSetting : Setting
    {
        // Objects & Variables
        private string _value;
        public string value {
            get => _value;
            set {
                _value = value;
                OnValueChanged(value);
            }
        }

        [JsonIgnore] public string defaultValue;
        [JsonIgnore] public string values;
        [JsonIgnore] public Action<string> OnValueChanged = (string newValue) => { };

        // Public Functions

        public override void RestoreDefault() {
            value = defaultValue;
        }

        // Constructors

        public ComboSetting() : base("Combo") { }
        public ComboSetting(string _default) : base("Combo") {
            value = _default;
            defaultValue = _default;
        }
    }

    public class IntSetting : Setting
    {
        // Objects & Variables
        private int _value;
        public int value {
            get => _value;
            set {
                _value = value;
                OnValueChanged(value);
            }
        }

        [JsonIgnore] public int min;
        [JsonIgnore] public int max;
        [JsonIgnore] public int defaultValue;
        [JsonIgnore] public Action<int> OnValueChanged = (int newValue) => { };

        // Public Functions

        public override void RestoreDefault() {
            value = defaultValue;
        }

        // Constructors

        public IntSetting() : base("Int") { }
        public IntSetting(int _default) : base("Int") {
            value = _default;
            defaultValue = _default;
        }
    }

    public class BoolSetting : Setting
    {
        // Objects & Variables
        private bool _value;
        public bool value {
            get => _value;
            set {
                _value = value;
                OnValueChanged(value);
            }
        }

        [JsonIgnore] public bool defaultValue;
        [JsonIgnore] public Action<bool> OnValueChanged = (bool newValue) => { };

        // Public Functions

        public override void RestoreDefault() {
            _value = defaultValue;
            value = defaultValue;
        }

        // Constructors

        public BoolSetting() : base("Bool") { }
        public BoolSetting(bool _default) : base("Bool") {
            value = _default;
            defaultValue = _default;
        }
    }

    public class ButtonSetting : Setting
    {
        // Objects & Variables
        [JsonIgnore] public string buttonText;
        [JsonIgnore] public Action OnClick = delegate () { };

        // Constructors

        public ButtonSetting() : base("Button") { }
    }

    public class ColourSetting : Setting
    {
        // Objects & Variables
        private Color _value;
        public Color value {
            get => _value;
            set {
                _value = value;
                OnValueChanged(value);
            }
        }

        [JsonIgnore] public Color defaultValue;
        [JsonIgnore] public Action<Color> OnValueChanged = (Color newValue) => { };

        // Public Functions

        public override void RestoreDefault() {
            value = defaultValue;
        }

        // Constructors

        public ColourSetting() : base("Colour") { }
        public ColourSetting(Color _default) : base("Colour") {
            value = _default;
            defaultValue = _default;
        }
    }
}
