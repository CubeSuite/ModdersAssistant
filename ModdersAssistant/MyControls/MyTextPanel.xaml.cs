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

namespace ModdersAssistant.MyControls
{
    /// <summary>
    /// Interaction logic for MyTextPanel.xaml
    /// </summary>
    public partial class MyTextPanel : UserControl
    {
        public MyTextPanel() {
            InitializeComponent();
        }

        // Properties

        #region Input Property

        public static readonly DependencyProperty InputProperty = DependencyProperty.Register("Input", typeof(string), typeof(MyTextPanel), new PropertyMetadata("", onInputChanged));

        public string Input {
            get => (string)GetValue(InputProperty);
            set {
                SetValue(InputProperty, value);
                onInputChanged(this, new DependencyPropertyChangedEventArgs(InputProperty, value, value));
            }
        }

        private static void onInputChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            MyTextPanel thisMyTextPanel = obj as MyTextPanel;
            thisMyTextPanel.TextChanged?.Invoke(thisMyTextPanel, EventArgs.Empty);
        }

        #endregion

        #region Hint Property

        public static readonly DependencyProperty HintProperty = DependencyProperty.Register("Hint", typeof(string), typeof(MyTextPanel), new PropertyMetadata("Hint..."));

        public string Hint {
            get => (string)GetValue(HintProperty);
            set => SetValue(HintProperty, value);
        }

        #endregion

        // Custom Events

        public event EventHandler EnterPressed;
        public event EventHandler EscapePressed;
        public event EventHandler TextChanged;
        public event EventHandler ChangesConfirmed;

        // Events

        public void OnInputBoxPreviewKeyUp(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                EnterPressed?.Invoke(this, EventArgs.Empty);
                ChangesConfirmed?.Invoke(this, EventArgs.Empty);
            }
            else if (e.Key == Key.Escape) {
                EscapePressed?.Invoke(this, EventArgs.Empty);
                ChangesConfirmed?.Invoke(this, EventArgs.Empty);
                Keyboard.ClearFocus();
            }
        }

        public void OnInputBoxLostFocus(object sender, KeyboardFocusChangedEventArgs e) {
            ChangesConfirmed?.Invoke(this, EventArgs.Empty);
        }
    }
}
