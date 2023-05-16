using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Uniza.Namedays;

namespace EditorGuiApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly NamedayCalendar _namedayCalendar = new();
        private ObservableCollection<string> SelectedNames { get; } = new();

        public MainWindow()
        {
            InitializeComponent();
            _namedayCalendar.Load(new FileInfo("namedays-sk.csv"));
            SelectedDateListBox.ItemsSource = SelectedNames;

            var names = _namedayCalendar[DateTime.Today];
            foreach (var name in names)
            {
                SelectedNames.Add(name);
            }

            SelectedDateTextBox.Clear();
            SelectedDateTextBox.SelectedText = $"{DateTime.Today.ToShortDateString()} celebrates:";
        }

        private void Calendar_OnDayButtonClick(object sender, SelectionChangedEventArgs e)
        {
            if (sender is Calendar calendar)
            {
                DateTime selectedDate = calendar.SelectedDate ?? DateTime.MinValue;

                if (SelectedDateTextBox != null)
                {
                    SelectedDateTextBox.Clear();
                    SelectedDateTextBox.SelectedText = $"{selectedDate.ToShortDateString()} celebrates:";
                }

                if (SelectedDateListBox != null)
                {
                    SelectedNames.Clear();
                    var names = _namedayCalendar[selectedDate];
                    foreach (var name in names)
                    {
                        SelectedNames.Add(name);
                    }
                }
            }
        }

        private void TodayJumpButton_OnClick(object sender, RoutedEventArgs e)
        {
            Calendar.SelectedDate = DateTime.Today;
        }
    }
}
