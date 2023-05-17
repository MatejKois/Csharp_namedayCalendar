using System.IO;
using Microsoft.Win32;
using System.Windows;
using Uniza.Namedays;
using Uniza.Namedays.EditorGuiApp;

namespace EditorGuiApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly NamedayCalendar _namedayCalendar;
        private readonly CalendarPage _calendarPage;

        public MainWindow()
        {
            InitializeComponent();

            _namedayCalendar = new NamedayCalendar();

            _namedayCalendar.Load(new FileInfo("namedays-sk.csv")); // debug

            EditorFrame.Content = new EditorPage(ref _namedayCalendar);

            _calendarPage = new CalendarPage(ref _namedayCalendar);
            CalendarFrame.Content = _calendarPage;
        }

        private void NewMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (_namedayCalendar.GetNamedays().Length != 0)
            {
                var result = MessageBox.Show("Are you sure you want to reset the calendar?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    return;
                }
            }

            _namedayCalendar.Clear();
        }

        private void OpenMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (_namedayCalendar.GetNamedays().Length != 0)
            {
                var result = MessageBox.Show("Discard all unsaved changes?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    return;
                }
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFileName = openFileDialog.FileName;
                _namedayCalendar.Load(new System.IO.FileInfo(selectedFileName));
                _calendarPage.Refresh();
            }
        }

        private void SaveAsMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV Files (*.csv)|*.csv";
            if (saveFileDialog.ShowDialog() == true)
            {
                string selectedFileName = saveFileDialog.FileName;
                _namedayCalendar.Save(new System.IO.FileInfo(selectedFileName));
            }
        }

        private void ExitMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Shows information about app via messagebox
        /// </summary>
        private void AboutMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            string applicationName = "Namedays";
            string version = "1.0";
            string name = "Stefan Zub";
            int creationYear = 2023;
            string description = "This application is designed for editing and viewing namedays.";

            string message = $"{applicationName} v{version}\n\n" +
                             $"Copyright © {name} {creationYear}\n\n" +
                             $"{description}";

            MessageBox.Show(message, "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
