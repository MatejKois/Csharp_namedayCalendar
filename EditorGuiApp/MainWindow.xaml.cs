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
        /// <summary>
        /// NamedayCalendar - a calendar shared by all the app components
        /// </summary>
        private readonly NamedayCalendar _namedayCalendar;

        /// <summary>
        /// CalendarPage gui component
        /// </summary>
        private readonly CalendarPage _calendarPage;

        /// <summary>
        /// EditorPage gui component
        /// </summary>
        private readonly EditorPage _editorPage;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            _namedayCalendar = new NamedayCalendar();

            _calendarPage = new CalendarPage(ref _namedayCalendar);
            _editorPage = new EditorPage(ref _namedayCalendar, ref _calendarPage);

            CalendarFrame.Content = _calendarPage;
            EditorFrame.Content = _editorPage;
        }

        /// <summary>
        /// Resets the calendar
        /// </summary>
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

        /// <summary>
        /// Provides support for loading the calendar from a .csv file
        /// </summary>
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
                _editorPage.RefreshFilterOutput();
            }
        }

        /// <summary>
        /// Provides support for saving the calendar to a .csv file
        /// </summary>
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

        /// <summary>
        /// Exits the app
        /// </summary>
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
            string name = "Matej Kois";
            int creationYear = 2023;
            string description = "This application is designed for editing and viewing namedays.";

            string message = $"{applicationName} v{version}\n\n" +
                             $"Copyright © {name} {creationYear}\n\n" +
                             $"{description}";

            MessageBox.Show(message, "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
