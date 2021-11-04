using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DesktopUI
{
    /// <summary>
    /// Interaction logic for AddToPlayedWindow.xaml
    /// </summary>
    public partial class AddToPlayedWindow : Window
    {
        public AddToPlayedWindow(MainVM parentVM, GameBL.PlayedGame game = null)
        {
            InitializeComponent();

            Loaded += MyWindow_Loaded;

            this.DataContext = new AddToPlayedVM(this, parentVM, game);
            (DataContext as AddToPlayedVM).CloseWindowEvent += CommandBench_CloseWindowEvent;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void MyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            (DataContext as AddToPlayedVM).UpdateRating();
        }

        private void CommandBench_CloseWindowEvent(object sender, EventArgs e)
        {
            this.DataContext = null;
            Close();
        }

        public void SetRating(int rating)
        {
            if (rating == 0)
            {
                r1.IsChecked = true;
            }
            else if (rating == 1)
            {
                r2.IsChecked = true;
            }
            else if (rating == 2)
            {
                r3.IsChecked = true;
            }
            else if (rating == 3)
            {
                r4.IsChecked = true;
            }

        }

        public void SetRatingEvent(int rating)
        {
            if (rating == 0)
            {
                r2.IsChecked = false;
                r3.IsChecked = false;
                r4.IsChecked = false;
                SetVMRating(rating);
            }
            else if (rating == 1)
            {
                r1.IsChecked = false;
                r3.IsChecked = false;
                r4.IsChecked = false;
                SetVMRating(rating);
            }
            else if (rating == 2)
            {
                r1.IsChecked = false;
                r2.IsChecked = false;
                r4.IsChecked = false;
                SetVMRating(rating);
            }
            else if (rating == 3)
            {
                r1.IsChecked = false;
                r2.IsChecked = false;
                r3.IsChecked = false;
                SetVMRating(rating);
            }
        }

        private void SetVMRating(int rating)
        {
            var dc = DataContext as AddToPlayedVM;
            dc.Rating = rating;
        }

        private void r1_Checked(object sender, RoutedEventArgs e)
        {
            SetRatingEvent(0);

        }

        private void r2_Checked(object sender, RoutedEventArgs e)
        {
            SetRatingEvent(1);

        }

        private void r3_Checked(object sender, RoutedEventArgs e)
        {
            SetRatingEvent(2);
        }

        private void r4_Checked(object sender, RoutedEventArgs e)
        {
            SetRatingEvent(3);
        }
        
        private void r1_Unchecked(object sender, RoutedEventArgs e)
        {
           // r1.IsChecked = true;
        }

        private void r_Unchecked(object sender, RoutedEventArgs e)
        {
            if(!(bool)r1.IsChecked && !(bool)r2.IsChecked && !(bool)r3.IsChecked && !(bool)r4.IsChecked)
                r1.IsChecked = true;
        }

        private void HoursTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
