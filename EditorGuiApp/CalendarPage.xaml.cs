using System;
using System.Collections.ObjectModel;
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
        private readonly NamedayCalendar _namedayCalendar = new();
        private ObservableCollection<string> SelectedNames { get; } = new();

        public CalendarPage()
        {
            InitializeComponent();

            _namedayCalendar.Load(new FileInfo("namedays-sk.csv"));
            SelectedDateListBox.ItemsSource = SelectedNames;

            UpdateSelectedNames(DateTime.Today);
            UpdateSelectedDateTextBox(DateTime.Today);
        }

        private void Calendar_OnDayButtonClick(object sender, SelectionChangedEventArgs e)
        {
            if (sender is Calendar calendar)
            {
                DateTime selectedDate = calendar.SelectedDate ?? DateTime.MinValue;

                UpdateSelectedDateTextBox(selectedDate);
                UpdateSelectedNames(selectedDate);
            }
        }

        private void UpdateSelectedNames(DateTime selectedDate)
        {
            SelectedNames.Clear();
            var names = _namedayCalendar[selectedDate];
            foreach (var name in names)
            {
                SelectedNames.Add(name);
            }
        }

        private void UpdateSelectedDateTextBox(DateTime selectedDate)
        {
            if (SelectedDateTextBox != null)
            {
                SelectedDateTextBox.Clear();
                SelectedDateTextBox.SelectedText = $"{selectedDate.ToShortDateString()} celebrates:";
            }
        }

        private void TodayJumpButton_OnClick(object sender, RoutedEventArgs e)
        {
            Calendar.SelectedDate = DateTime.Today;
        }
    }
}
