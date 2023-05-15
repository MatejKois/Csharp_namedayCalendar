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

namespace EditorGuiApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<string> Namedays { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Calendar_OnDayButtonClick(object sender, SelectionChangedEventArgs e)
        {
            DateTime selectedDate = Calendar.SelectedDate ?? DateTime.MinValue;
            MessageBox.Show($"Selected date: {selectedDate.ToShortDateString()}");
        }
    }
}
