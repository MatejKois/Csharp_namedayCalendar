using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Uniza.Namedays.EditorGuiApp
{
    /// <summary>
    /// Interaction logic for CalendarPage.xaml
    /// </summary>
    public partial class CalendarPage : Page
    {
        private NamedayCalendar NamedayCalendar { get; }
        private ObservableCollection<string> SelectedNames { get; } = new();

        public CalendarPage(ref NamedayCalendar namedayCalendar)
        {
            InitializeComponent();

            NamedayCalendar = namedayCalendar;
            SelectedDateListBox.ItemsSource = SelectedNames;

            Calendar.SelectedDate = DateTime.Today;

            Refresh();
        }

        private void Calendar_OnDayButtonClick(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

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

        private void TodayJumpButton_OnClick(object sender, RoutedEventArgs e)
        {
            Calendar.SelectedDate = DateTime.Today;
            Calendar.DisplayDate = DateTime.Today;
            Refresh();
        }
    }
}
