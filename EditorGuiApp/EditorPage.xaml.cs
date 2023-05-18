using EditorGuiApp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Uniza.Namedays.EditorGuiApp
{
    /// <summary>
    /// Interaction logic for EditorPage.xaml
    /// </summary>
    public partial class EditorPage : Page
    {
        /// <summary>
        /// NamedayCalendar - is initialized as a reference to a calendar shared by all the app components
        /// </summary>
        private NamedayCalendar NamedayCalendar { get; set; }

        /// <summary>
        /// Contains namedays to display in the listbox based on the filter selection
        /// </summary>
        private ObservableCollection<Nameday> NamedaysInFilterOutput { get; } = new();

        /// <summary>
        /// Reference to the calendar page
        /// </summary>
        private readonly CalendarPage _calendarPage;

        /// <summary>
        /// Month selected in the filter
        /// </summary>
        private int FilterMonth { get; set; }

        /// <summary>
        /// Regex to filter the namedays names with
        /// </summary>
        private string FilterRegex { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="namedayCalendar">Reference to a calendar shared by all the app components</param>
        public EditorPage(ref NamedayCalendar namedayCalendar, ref CalendarPage calendarPage)
        {
            InitializeComponent();

            FilterMonth = 0;
            FilterRegex = "";

            NamedayCalendar = namedayCalendar;
            _calendarPage = calendarPage;

            FilterOutputListBox.ItemsSource = NamedaysInFilterOutput;

            EditButton.IsEnabled = false;
            RemoveButton.IsEnabled = false;
            ShowOnCalendarButton.IsEnabled = false;

            CountButton.Text = $"Count: - / {NamedayCalendar.NameCount}";

            RefreshFilterOutput();
        }

        /// <summary>
        /// Is called when user selects different month in the filter
        /// </summary>
        private void Filter_OnMonthSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedMonthItem = (ComboBoxItem)comboBox.SelectedItem;

            try
            {
                if (selectedMonthItem != null)
                {
                    string? selectedMonth = selectedMonthItem.Content.ToString();
                    if (selectedMonth != null)
                    {
                        FilterMonth = DateTime.ParseExact(selectedMonth, "MMMM", CultureInfo.InvariantCulture).Month;
                    }
                }
                else
                {
                    FilterMonth = 0;
                }
            }
            catch (Exception)
            {
                FilterMonth = 0;
            }

            RefreshFilterOutput();
        }

        /// <summary>
        /// Is called whenever the regex input in the filter is changed
        /// </summary>
        private void Filter_OnFilterInputChanged(object sender, RoutedEventArgs e)
        {
            FilterRegex = RegexFilterInput.Text;
            RefreshFilterOutput();
        }

        private void Editor_OnListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedNameday = FilterOutputListBox.SelectedItem as Nameday?;

            if (selectedNameday is null)
            {
                EditButton.IsEnabled = false;
                RemoveButton.IsEnabled = false;
                ShowOnCalendarButton.IsEnabled = false;

                CountButton.Text = $"Count: - / {NamedayCalendar.NameCount}";

                return;
            }

            EditButton.IsEnabled = true;
            RemoveButton.IsEnabled = true;
            ShowOnCalendarButton.IsEnabled = true;

            CountButton.Text = $"Count: {Array.IndexOf(NamedayCalendar.GetNamedays(), selectedNameday.Value) + 1} / {NamedayCalendar.NameCount}";
        }

        /// <summary>
        /// Is called on filter clear button press
        /// </summary>
        private void Filter_OnFilterClearPressed(object sender, RoutedEventArgs e)
        {
            var defaultSelection = SelectedMonthBox.Items.Cast<object>().FirstOrDefault(item => item.ToString() == "");
            SelectedMonthBox.SelectedItem = defaultSelection;
            RegexFilterInput.Text = "";

            RefreshFilterOutput();
        }

        /// <summary>
        /// Refreshes the output from the filter in the listbox
        /// </summary>
        private void RefreshFilterOutput()
        {
            try
            {
                var namedays = NamedayCalendar.GetNamedays(FilterRegex).Where(nd => FilterMonth == 0 || nd.DayMonth.Month.Equals(FilterMonth));

                NamedaysInFilterOutput.Clear();

                foreach (var nameday in namedays)
                {
                    NamedaysInFilterOutput.Add(nameday);
                }
            }
            catch (Exception)
            {
                // ignored, sometimes user enters forbidden regex characters and Exception is thrown on "var namedays" query, output won't refresh in that case
            }
            _calendarPage.Refresh();
        }

        private void Editor_OnAddButtonPressed(object sender, RoutedEventArgs e)
        {
            var nameAddWindow = new NamedayWindow();
            nameAddWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            nameAddWindow.Owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);

            if (nameAddWindow.ShowDialog() == true)
            {
                var nameday = new Nameday(nameAddWindow.EnteredName, new DayMonth(nameAddWindow.NamedayDate.Day, nameAddWindow.NamedayDate.Month));
                NamedayCalendar.Add(nameday);
                NamedayCalendar.Sort();
                RefreshFilterOutput();
            }
        }

        private void Editor_OnEditButtonPressed(object sender, RoutedEventArgs e)
        {
            var selectedNameday = FilterOutputListBox.SelectedItem as Nameday?;

            if (selectedNameday is null) {
                return;
            }

            var nameEditWindow = new NamedayWindow();
            nameEditWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            nameEditWindow.Owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);

            nameEditWindow.EnteredName = selectedNameday.Value.Name;
            nameEditWindow.NamedayDate = selectedNameday.Value.DayMonth.ToDateTime();

            if (nameEditWindow.ShowDialog() == true)
            {
                NamedayCalendar.Update(selectedNameday.Value, nameEditWindow.EnteredName, nameEditWindow.NamedayDate);
                NamedayCalendar.Sort();
                RefreshFilterOutput();
            }
        }

        private void Editor_OnRemoveButtonPressed(object sender, RoutedEventArgs e)
        {
            var selectedNameday = FilterOutputListBox.SelectedItem as Nameday?;

            if (selectedNameday is null)
            {
                return;
            }

            var result = MessageBox.Show("Delete the selected nameday?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                return;
            }

            NamedayCalendar.Remove(selectedNameday.Value.Name);
            RefreshFilterOutput();
        }

        private void Editor_OnShowOnCalendarButtonPressed(object sender, RoutedEventArgs e)
        {
            var selectedNameday = FilterOutputListBox.SelectedItem as Nameday?;

            if (selectedNameday is null)
            {
                return;
            }

            _calendarPage.Calendar.SelectedDate = selectedNameday.Value.DayMonth.ToDateTime();
            _calendarPage.Calendar.DisplayDate = selectedNameday.Value.DayMonth.ToDateTime();
            _calendarPage.Refresh();
        }
    }
}
