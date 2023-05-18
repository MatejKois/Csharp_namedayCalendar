using System;
using System.Windows;

namespace Uniza.Namedays.EditorGuiApp
{
    /// <summary>
    /// Interaction logic for NameAddWindow.xaml
    /// </summary>
    public partial class NamedayWindow : Window
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NamedayWindow()
        {
            InitializeComponent();

            SizeToContent = SizeToContent.WidthAndHeight;
            MinWidth = Width;
            MinHeight = Height;
        }

        /// <summary>
        /// Get or set name
        /// </summary>
        public string EnteredName
        {
            get => NameTextBox.Text;
            set => NameTextBox.Text = value;
        }

        /// <summary>
        /// Get or set nameday date
        /// </summary>
        public DateTime NamedayDate
        {
            get => NamedayDatePicker.SelectedDate ?? DateTime.Now;
            set => NamedayDatePicker.SelectedDate = value;
        }

        /// <summary>
        /// Is called on ok button press
        /// </summary>
        private void OKButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
