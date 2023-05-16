using System.Windows;
using Uniza.Namedays.EditorGuiApp;

namespace EditorGuiApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CalendarFrame.Content = new CalendarPage();
        }
    }
}
