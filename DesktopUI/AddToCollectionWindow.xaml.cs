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
    /// Interaction logic for AddToCollectionWindow.xaml
    /// </summary>
    public partial class AddToCollectionWindow : Window
    {


        public AddToCollectionWindow(MainVM parentVM, GameBL.CollectionGame game = null)
        {
            InitializeComponent();
            Loaded += MyWindow_Loaded;

            this.DataContext = new AddToCollectionVM(parentVM, game);
            (DataContext as AddToCollectionVM).CloseWindowEvent += CommandBench_CloseWindowEvent;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;



            if (game != null) // edit mode, make window smaller
                this.Height = 250;
        }

        private void MyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //       (DataContext as AddToCollectionVM).Load();
        }

        private void CommandBench_CloseWindowEvent(object sender, EventArgs e)
        {
            this.DataContext = null;
            Close();
        }



        private void ComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            (DataContext as AddToCollectionVM).NameUnfocused();
        }

        private void YearReleased_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
