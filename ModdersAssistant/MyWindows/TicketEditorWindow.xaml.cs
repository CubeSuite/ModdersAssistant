using ModdersAssistant.MyClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ModdersAssistant.MyWindows
{
    /// <summary>
    /// Interaction logic for TicketEditorWindow.xaml
    /// </summary>
    public partial class TicketEditorWindow : Window
    {
        public TicketEditorWindow() {
            Owner = Application.Current.MainWindow;
            InitializeComponent();
            Width = ProgramData.programWidth;
            Height = ProgramData.programHeight;
        }

        // Properties

        // Objects & Variables

        Ticket ticket;
        private bool finishedLoading = false;

        // Custom Events

        // Events

        private void OnTitleBoxTextChanged(object sender, EventArgs e) {
            if (!finishedLoading) return;

            string newTitle = titleBox.Input;
            if (string.IsNullOrEmpty(newTitle)) {
                GuiUtils.ShowErrorMessage("Invalid Title", "Ticket title cannot be blank");
                return;
            }

            Log.Debug($"Updating title of {ticket} to '{newTitle}'");
            ticket.title = newTitle;
        }

        private void OnTypeBoxSelectedItemChanged(object sender, EventArgs e) {
            if (!finishedLoading) return;
            Log.Debug($"Updating type for {ticket} to {typeBox.SelectedItem}");
            ticket.type = StringUtils.GetTicketTypeFromName(typeBox.SelectedItem);
        }

        private void OnDescriptionBoxTextChanged(object sender, EventArgs e) {
            if (!finishedLoading) return;
            Log.Debug($"Updating description for {ticket}");
            ticket.description = descriptionBox.Input;
        }

        private void OnConfirmClicked(object sender, EventArgs e) {
            Close();
        }

        private void OnCancelClicked(object sender, EventArgs e) {
            ticket = null;
            Close();
        }

        // Private Functions

        private void ShowTicket(Ticket ticketToShow) {
            ticket = ticketToShow;
            typeBox.SetItems(StringUtils.GetAllTicketTypeNamesForCombo());

            titleBox.Input = ticket.title;
            typeBox.SetItem(StringUtils.GetTicketTypeName(ticket.type));
            descriptionBox.Input = ticket.description;
            descriptionBox.inputBox.TextWrapping = TextWrapping.Wrap;
            descriptionBox.inputBox.MaxWidth = 780;
            finishedLoading = true;
        }

        // Return Functions

        private Ticket GetTicket() { return ticket; }

        public static Ticket CreateTicket() {
            TicketEditorWindow window = new TicketEditorWindow();
            window.ShowTicket(new Ticket());
            window.ShowDialog();
            return window.GetTicket();
        }

        public static Ticket EditTicket(Ticket ticketToEdit) {
            TicketEditorWindow window = new TicketEditorWindow();
            window.ShowTicket(ticketToEdit);
            window.ShowDialog();
            return window.GetTicket();
        }
    }
}
