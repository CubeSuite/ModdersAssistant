using ModdersAssistant.MyClasses;
using ModdersAssistant.MyWindows;
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

namespace ModdersAssistant.MyPanels
{
    /// <summary>
    /// Interaction logic for TicketPanel.xaml
    /// </summary>
    public partial class TicketPanel : UserControl
    {
        public TicketPanel() {
            InitializeComponent();
        }

        public TicketPanel(ref Ticket ticketToShow) {
            InitializeComponent();
            ShowTicket(ref ticketToShow);
        }

        // Objects & Variables
        public Ticket ticket;
        private bool finishedLoading = false;

        // Custom Events

        public event EventHandler CompletedToggled;

        // Events

        private void OnCompleteBoxToggled(object sender, EventArgs e) {
            if (!finishedLoading) return;
            ticket.complete = completeBox.IsChecked;
            CompletedToggled?.Invoke(this, EventArgs.Empty);
        }

        private void OnEditClicked(object sender, EventArgs e) {
            Ticket updatedTicket = TicketEditorWindow.EditTicket(ticket);
            if(updatedTicket == null) {
                Log.Debug("User canceled ticket editing");
                return;
            }

            ticket.title = updatedTicket.title;
            ticket.type = updatedTicket.type;
            ticket.description = updatedTicket.description;

            ShowTicket(ref ticket);
        }

        // Private Functions

        private void ShowTicket(ref Ticket ticketToShow) {
            ticket = ticketToShow;
            completeBox.IsChecked = ticket.complete;

            switch (ticket.type) {
                case TicketType.NewFeature: titleLabel.Text = $"F - {ticket.title}"; break;
                case TicketType.Change: titleLabel.Text = $"C - {ticket.title}"; break;
                case TicketType.Bug: titleLabel.Text = $"B - {ticket.title}"; break;
            }
            
            finishedLoading = true;
        }
    }
}