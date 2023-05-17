using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Uniza.Namedays.EditorGuiApp
{
    /// <summary>
    /// Interaction logic for CalendarPage.xaml
    /// </summary>
    public partial class CalendarPage : Page
    {
        /// <summary>
        /// NamedayCalendar - is initialized as a reference to a calendar shared by all the app components
        /// </summary>
        private NamedayCalendar NamedayCalendar { get; }

        /// <summary>
        /// Contains names to display in the list box based on the selected date from the calendar
        /// </summary>
        private ObservableCollection<string> SelectedNames { get; } = new();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="namedayCalendar">Reference to a calendar shared by all the app components</param>
        public CalendarPage(ref NamedayCalendar namedayCalendar)
        {
            InitializeComponent();

            NamedayCalendar = namedayCalendar;
            SelectedDateListBox.ItemsSource = SelectedNames;

            Calendar.SelectedDate = DateTime.Today;

            Refresh();
        }

        /// <summary>
        /// Calls the refresh method when user selects a date
        /// </summary>
        private void Calendar_OnDayButtonClick(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        /// <summary>
        /// Refreshes the displayed / selected values in the page
        /// </summary>
        public void Refresh()
        {
            DateTime selectedDate = Calendar.SelectedDate ?? DateTime.MinValue;

            // update listbox
            if (NamedayCalendar == null)
            {
                return;
            }
            SelectedNames.Clear();
            var names = NamedayCalendar[selectedDate];
            foreach (var name in names)
            {
                SelectedNames.Add(name);
            }

            // update textbox
            if (SelectedDateTextBox != null)
            {
                SelectedDateTextBox.Clear();
                SelectedDateTextBox.SelectedText = $"{selectedDate.ToShortDateString()} celebrates:";
            }
        }

        /// <summary>
        /// Jumps the calendar to today
        /// </summary>
        private void TodayJumpButton_OnClick(object sender, RoutedEventArgs e)
        {
            Calendar.SelectedDate = DateTime.Today;
            Calendar.DisplayDate = DateTime.Today;
            Refresh();
        }
    }
}
