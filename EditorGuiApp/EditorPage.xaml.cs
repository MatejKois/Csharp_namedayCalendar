using System;
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
        private NamedayCalendar NamedayCalendar { get; }
        private ObservableCollection<Nameday> NamedaysInFilterOutput { get; } = new();
        private int FilterMonth { get; set; }
        private string FilterRegex { get; set; }

        public EditorPage(ref NamedayCalendar namedayCalendar)
        {
            InitializeComponent();

            FilterMonth = 0;
            FilterRegex = "";

            NamedayCalendar = namedayCalendar;
            FilterOutputListBox.ItemsSource = NamedaysInFilterOutput;

            RefreshFilterOutput();
        }

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

        private void Filter_OnFilterInputChanged(object sender, RoutedEventArgs e)
        {
            FilterRegex = RegexFilterInput.Text;
            RefreshFilterOutput();
        }

        private void Filter_OnFilterClearPressed(object sender, RoutedEventArgs e)
        {
            var defaultSelection = SelectedMonthBox.Items.Cast<object>().FirstOrDefault(item => item.ToString() == "");
            SelectedMonthBox.SelectedItem = defaultSelection;
            RegexFilterInput.Text = "";

            RefreshFilterOutput();
        }

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
                // ignored, sometimes user enters forbidden regex characters, output won't refresh in that case
            }
        }
    }
}
